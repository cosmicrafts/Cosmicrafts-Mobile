using UnityEngine;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;
using Unity.Mathematics;
using Unity.NetCode;

[BurstCompile]
[UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
public partial struct PlayerMovementSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        var builder = new EntityQueryBuilder(Allocator.Temp);
        // It needs to have all 3 in order to execute PD, PID, LT
        builder.WithAll<PlayerData, PlayerInputData, LocalTransform>();
        state.RequireForUpdate(state.GetEntityQuery(builder));
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var job = new PlayerMovementJob
        {
            deltaTime = SystemAPI.Time.DeltaTime
        };
        state.Dependency = job.ScheduleParallel(state.Dependency);
    }
}

[BurstCompile]
public partial struct PlayerMovementJob : IJobEntity
{
    public float deltaTime;
    
    public void Execute(PlayerData player, PlayerInputData input, ref LocalTransform transform)
    {
        // Adjust for 2D movement (x and y only, z remains 0)
        float2 movement = new float2(input.move.x, input.move.y) * player.speed * deltaTime;

        // Directly update the 2D position on the top-down plane
        transform.Position = new float3(transform.Position.x + movement.x, transform.Position.y + movement.y, 0f);
    }
}
