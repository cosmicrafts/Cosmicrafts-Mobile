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
        
        var playerTeams = playerQuery.ToComponentDataArray<Team>(Allocator.TempJob);
        var playerTransforms = playerQuery.ToComponentDataArray<LocalTransform>(Allocator.TempJob);

        // Schedule a job that finds the target player for each unit (sequentially)
        JobHandle jobHandle = Entities
            .WithAll<UnitData, Team>()
            .ForEach((ref TargetPosition targetPosition, in Team unitTeam) => 
            {
                // Find the opposing team's player
                for (int i = 0; i < playerTeams.Length; i++)
                {
                    if (playerTeams[i].Value != unitTeam.Value) 
                    {
                        targetPosition.Value = playerTransforms[i].Position;
                        break;
                    }
                }
            }).Schedule(Dependency); // Use `Schedule()` for sequential processing

        Dependency = jobHandle;
        jobHandle.Complete();

        // Dispose of arrays after job completion
        playerTeams.Dispose();
        playerTransforms.Dispose();
    }
}
