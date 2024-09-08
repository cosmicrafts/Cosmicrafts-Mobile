using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Jobs;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public partial class FindTargetPlayerSystem : SystemBase
{
    protected override void OnUpdate()
    {
        // Get player data and transforms
        var playerQuery = GetEntityQuery(ComponentType.ReadOnly<PlayerData>(), 
                                         ComponentType.ReadOnly<Team>(),
                                         ComponentType.ReadOnly<LocalTransform>());
        
        var players = playerQuery.ToComponentDataArray<PlayerData>(Allocator.TempJob);
        var playerTeams = playerQuery.ToComponentDataArray<Team>(Allocator.TempJob);
        var playerTransforms = playerQuery.ToComponentDataArray<LocalTransform>(Allocator.TempJob);

        // Schedule a job that finds the target player for each unit
        JobHandle jobHandle = Entities
            .WithAll<UnitData, Team>()
            .ForEach((ref TargetPosition targetPosition, in Team unitTeam) => 
            {
                // Find the opposing team's player
                for (int i = 0; i < players.Length; i++)
                {
                    if (playerTeams[i].Value != unitTeam.Value) 
                    {
                        targetPosition.Value = playerTransforms[i].Position;
                        break;
                    }
                }
            }).ScheduleParallel(Dependency);

        // Ensure the job completes before disposing of the native arrays
        jobHandle.Complete();

        players.Dispose();
        playerTeams.Dispose();
        playerTransforms.Dispose();
    }
}
