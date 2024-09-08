using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;  // For Quaternion

[BurstCompile]
public partial struct UnitRotationSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<UnitData>();
    }

    public void OnUpdate(ref SystemState state)
    {
        var deltaTime = SystemAPI.Time.DeltaTime;

        // Update each unit's rotation based on its movement direction (TargetPosition)
        foreach (var (transform, targetPosition) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<TargetPosition>>())
        {
            float3 direction = math.normalize(targetPosition.ValueRO.Value - transform.ValueRW.Position);

            // If there's significant movement, rotate the unit to face the direction
            if (math.lengthsq(direction) > 0.01f)
            {
                quaternion targetRotation = quaternion.LookRotationSafe(direction, math.up());

                // Smoothly rotate towards the target direction
                transform.ValueRW.Rotation = math.slerp(transform.ValueRW.Rotation, targetRotation, deltaTime * 12f); // You can adjust the speed
            }
        }
    }
}
