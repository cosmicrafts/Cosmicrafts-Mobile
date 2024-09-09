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
        // Get player data and transforms using ComponentLookup for direct access
        var teamLookup = GetComponentLookup<Team>(true);
        var transformLookup = GetComponentLookup<LocalTransform>(true);

        // Get player entities and teams
        var playerQuery = GetEntityQuery(ComponentType.ReadOnly<PlayerData>(), 
                                         ComponentType.ReadOnly<Team>(),
                                         ComponentType.ReadOnly<LocalTransform>());

        var playerEntities = playerQuery.ToEntityArray(Allocator.TempJob);

        // Schedule a job that finds the target player for each unit (sequentially)
        JobHandle jobHandle = Entities
            .WithAll<UnitData, Team>()
            .ForEach((ref TargetPosition targetPosition, in Team unitTeam) => 
            {
                // Iterate through the players to find an opposing team member
                for (int i = 0; i < playerEntities.Length; i++)
                {
                    var playerEntity = playerEntities[i];
                    var playerTeam = teamLookup[playerEntity];

                    // Find an opposing player
                    if (playerTeam.Value != unitTeam.Value) 
                    {
                        targetPosition.Value = transformLookup[playerEntity].Position;
                        break; // Stop after finding the first opposing player
                    }
                }
            }).Schedule(Dependency); // Schedule job for parallel execution

        Dependency = jobHandle;

        // Dispose of arrays after job completion
        playerEntities.Dispose(jobHandle);
    }
}
