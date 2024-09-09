using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Jobs;
using Unity.NetCode;

[BurstCompile]
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public partial class FindTargetPlayerSystem : SystemBase
{
    [BurstCompile]
    protected override void OnUpdate()
    {
        // Get the lookup tables for Team and LocalTransform
        var teamLookup = GetComponentLookup<Team>(true);
        var transformLookup = GetComponentLookup<LocalTransform>(true);

        // Get all player entities
        var playerQuery = GetEntityQuery(ComponentType.ReadOnly<PlayerData>(), 
                                         ComponentType.ReadOnly<Team>(),
                                         ComponentType.ReadOnly<LocalTransform>());

        var playerEntities = playerQuery.ToEntityArray(Allocator.TempJob);

        // Cache each player's position into PlayerPosition component
        JobHandle jobHandle = Entities
            .WithAll<PlayerData>()
            .ForEach((Entity playerEntity, ref PlayerPosition playerPosition) => 
            {
                // Store the player's current position in PlayerPosition
                playerPosition.Value = transformLookup[playerEntity].Position;
            }).ScheduleParallel(Dependency); // Parallel job execution for efficiency

        Dependency = jobHandle;

        // Dispose of the player entities array
        playerEntities.Dispose(jobHandle);
    }
}
