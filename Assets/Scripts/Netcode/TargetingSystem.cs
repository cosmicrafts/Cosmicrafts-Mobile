using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Jobs;
using Unity.NetCode;
using UnityEngine;

[BurstCompile]
public partial struct TargetingSystem : ISystem
{
    private ComponentTypeHandle<LocalTransform> unitTransformType;
    private ComponentTypeHandle<CombatData> unitCombatType;
    private ComponentTypeHandle<Team> unitTeamType;
    private ComponentTypeHandle<UnitState> unitStateType;
    private EntityTypeHandle entityTypeHandle;

    // Add these for enemies as well
    private ComponentTypeHandle<LocalTransform> enemyTransformType;
    private ComponentTypeHandle<Team> enemyTeamType;

    private EntityQuery unitQuery;
    private EntityQuery enemyQuery;

    public void OnCreate(ref SystemState state)
    {
        // Initialize component handles in OnCreate
        unitTransformType = state.GetComponentTypeHandle<LocalTransform>(true);
        unitCombatType = state.GetComponentTypeHandle<CombatData>(true);
        unitTeamType = state.GetComponentTypeHandle<Team>(true);
        unitStateType = state.GetComponentTypeHandle<UnitState>(false);
        entityTypeHandle = state.GetEntityTypeHandle();

        // Initialize enemy component handles in OnCreate
        enemyTransformType = state.GetComponentTypeHandle<LocalTransform>(true);
        enemyTeamType = state.GetComponentTypeHandle<Team>(true);

        // Create queries in OnCreate
        unitQuery = SystemAPI.QueryBuilder()
                             .WithAll<LocalTransform, CombatData, Team, UnitState>()
                             .Build();

        enemyQuery = SystemAPI.QueryBuilder()
                             .WithAll<LocalTransform, Team>()
                             .Build();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // Update the component type handles in OnUpdate
        unitTransformType.Update(ref state);
        unitCombatType.Update(ref state);
        unitTeamType.Update(ref state);
        unitStateType.Update(ref state);
        entityTypeHandle.Update(ref state);

        // Update the enemy component type handles
        enemyTransformType.Update(ref state);
        enemyTeamType.Update(ref state);

        // Get the current time
        var time = SystemAPI.Time.ElapsedTime;

        var unitChunks = unitQuery.ToArchetypeChunkArray(Allocator.TempJob);
        var enemyChunks = enemyQuery.ToArchetypeChunkArray(Allocator.TempJob);

        var commandBuffer = new EntityCommandBuffer(Allocator.TempJob);

        // Setup the job
        var job = new TargetingJob
        {
            Time = (float)time,
            UnitTransformType = unitTransformType,
            UnitCombatType = unitCombatType,
            UnitTeamType = unitTeamType,
            UnitStateType = unitStateType,
            EntityTypeHandle = entityTypeHandle,
            EnemyChunks = enemyChunks,
            EnemyTransformType = enemyTransformType,
            EnemyTeamType = enemyTeamType,
            CommandBuffer = commandBuffer.AsParallelWriter(),
            DebugEntityManager = state.EntityManager
        };

        // Schedule the job in parallel
        var handle = job.ScheduleParallel(unitQuery, state.Dependency);
        state.Dependency = handle;

        state.Dependency.Complete();
        commandBuffer.Playback(state.EntityManager);
        commandBuffer.Dispose();

        unitChunks.Dispose();
        enemyChunks.Dispose();
    }

    [BurstCompile]
    struct TargetingJob : IJobChunk
    {
        public float Time;
        [ReadOnly] public ComponentTypeHandle<LocalTransform> UnitTransformType;
        [ReadOnly] public ComponentTypeHandle<CombatData> UnitCombatType;
        [ReadOnly] public ComponentTypeHandle<Team> UnitTeamType;
        public ComponentTypeHandle<UnitState> UnitStateType;
        [ReadOnly] public EntityTypeHandle EntityTypeHandle;

        [ReadOnly] public NativeArray<ArchetypeChunk> EnemyChunks;
        [ReadOnly] public ComponentTypeHandle<LocalTransform> EnemyTransformType;
        [ReadOnly] public ComponentTypeHandle<Team> EnemyTeamType;

        public EntityCommandBuffer.ParallelWriter CommandBuffer;

        [NativeDisableParallelForRestriction]
        [ReadOnly] public EntityManager DebugEntityManager;

        public void Execute(in ArchetypeChunk chunk, int chunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
        {
            var unitTransforms = chunk.GetNativeArray(ref UnitTransformType);
            var unitCombats = chunk.GetNativeArray(ref UnitCombatType);
            var unitTeams = chunk.GetNativeArray(ref UnitTeamType);
            var unitStates = chunk.GetNativeArray(ref UnitStateType);
            var entities = chunk.GetNativeArray(EntityTypeHandle);

            for (int i = 0; i < chunk.Count; i++)
            {
                if (useEnabledMask && !BitmaskHelper.IsSet(chunkEnabledMask, i)) 
                    continue;

                var unitEntity = entities[i];
                var unitCombat = unitCombats[i];
                var unitState = unitStates[i];
                float2 targetPosition = unitTransforms[i].Position.xy; // Use 2D position (x, y)
                bool hasTarget = false;

                foreach (var enemyChunk in EnemyChunks)
                {
                    var enemyTransforms = enemyChunk.GetNativeArray(ref EnemyTransformType);
                    var enemyTeams = enemyChunk.GetNativeArray(ref EnemyTeamType);

                    for (int j = 0; j < enemyChunk.Count; j++)
                    {
                        if (unitTeams[i].Value == enemyTeams[j].Value)
                            continue;

                        float distance = math.distance(unitTransforms[i].Position.xy, enemyTransforms[j].Position.xy);
                        if (distance <= unitCombat.DetectionRange)
                        {
                            hasTarget = true;
                            targetPosition = enemyTransforms[j].Position.xy;

                            // Ensure ProjectileData is only added if it doesn't already exist
                            if (!DebugEntityManager.HasComponent<ProjectileData>(unitEntity))
                            {
                                CommandBuffer.AddComponent(chunkIndex, unitEntity, new ProjectileData
                                {
                                    Damage = unitCombat.AttackDamage,
                                    Speed = 10f,
                                    Target = unitEntity
                                });
                            }

                            unitState.IsAttacking = true;
                            unitStates[i] = unitState;
                            CommandBuffer.SetComponent(chunkIndex, unitEntity, new TargetPosition { Value = new float3(targetPosition, 0f) }); // 2D target with z = 0
                            break;
                        }
                    }

                    if (hasTarget) break;
                }
            }
        }
    }

    // Helper for bitmask checking
    public static class BitmaskHelper
    {
        public static bool IsSet(v128 mask, int index)
        {
            return (mask.SInt0 & (1 << index)) != 0;
        }
    }
}
