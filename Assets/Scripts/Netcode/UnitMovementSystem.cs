using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct UnitMovementSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<UnitData>();
    }

    public void OnUpdate(ref SystemState state)
    {
        var deltaTime = SystemAPI.Time.DeltaTime;

        // Move each unit towards the target position
        foreach (var (transform, targetPosition, unitData) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<TargetPosition>, RefRO<UnitData>>())
        {
            // Calculate direction towards the target
            float3 direction = math.normalize(targetPosition.ValueRO.Value - transform.ValueRW.Position);

            // If direction is not zero, move the unit
            if (math.length(direction) > 0.01f)
            {
                float3 movement = direction * unitData.ValueRO.speed * deltaTime;
                transform.ValueRW.Position += movement;

                // For debugging, show the movement
                //Debug.Log($"Moving unit towards base at {targetPosition.ValueRO.Value}. New Position: {transform.ValueRW.Position}");
            }
        }
    }
}
