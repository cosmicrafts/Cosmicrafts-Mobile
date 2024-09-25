using UnityEngine;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;
using Unity.Mathematics;
using Unity.NetCode;

[BurstCompile]
[UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
public partial struct MovementSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        // Ensure the system runs only when UnitData is present (for units and players)
        state.RequireForUpdate<UnitData>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var deltaTime = SystemAPI.Time.DeltaTime;

        var job = new MovementJob
        {
            deltaTime = deltaTime
        };

        state.Dependency = job.ScheduleParallel(state.Dependency);
    }
}

[BurstCompile]
public partial struct MovementJob : IJobEntity
{
    public float deltaTime;

    public void Execute(
        ref LocalTransform transform,
        ref UnitState unitState,
        in TargetPosition targetPosition,
        in UnitData unitData)
    {
        // Calculate movement direction in 2D (x and y, ignoring z)
        float2 direction2D = targetPosition.Value.xy - transform.Position.xy;
        float lengthSquared = math.lengthsq(direction2D);

        // If there's significant movement, move and rotate the unit
        if (lengthSquared > 0.01f)
        {
            // Normalize direction for movement
            float2 normalizedDirection = math.normalize(direction2D);

            // Perform movement in 2D (updating x and y, leaving z as 0)
            float2 movement = normalizedDirection * unitData.speed * deltaTime;
            transform.Position = new float3(transform.Position.x + movement.x, transform.Position.y + movement.y, 0f);

            // Rotate the unit to face the direction it's moving
            float angle = math.atan2(normalizedDirection.y, normalizedDirection.x);
            quaternion targetRotation = quaternion.Euler(0f, 0f, angle); // Rotation around the Z-axis for 2D

            // Smoothly rotate towards the target direction
            transform.Rotation = math.slerp(transform.Rotation, targetRotation, deltaTime * 12f);

            // Update the unit state to reflect the movement
            unitState.IsMoving = true;
        }
        else
        {
            unitState.IsMoving = false;
        }
    }
}
