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
        //it needs to have all 3 in order to execute PD PID LT
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
        // Use X and Y for 2D movement
        float3 movement = new float3(input.move.x, input.move.y, 0) * player.speed * deltaTime;

        // Apply the movement to the player's position
        transform.Position = transform.Translate(movement).Position;
    }
}
