using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

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
            float2 direction = math.normalize(targetPosition.ValueRO.Value.xy - transform.ValueRW.Position.xy);

            // If there's significant movement, rotate the unit to face the direction
            if (math.lengthsq(direction) > 0.01f)
            {
                // Calculate the angle for 2D rotation in radians (relative to the positive x-axis)
                float angle = math.atan2(direction.y, direction.x);

                // Convert the angle into a quaternion for 2D (rotation only around the z-axis)
                quaternion targetRotation = quaternion.Euler(0, 0, angle);

                // Smoothly rotate towards the target direction
                transform.ValueRW.Rotation = math.slerp(transform.ValueRW.Rotation, targetRotation, deltaTime * 12f);
            }
        }
    }
}
