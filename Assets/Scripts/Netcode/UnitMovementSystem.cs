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

        foreach (var (transform, targetPosition, unitData, unitState) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<TargetPosition>, RefRO<UnitData>, RefRW<UnitState>>())
        {
            // Only move if the unit is not attacking
            if (!unitState.ValueRW.IsAttacking)
            {
                float3 direction = math.normalize(targetPosition.ValueRO.Value - transform.ValueRW.Position);
                if (math.length(direction) > 0.01f)
                {
                    float3 movement = direction * unitData.ValueRO.speed * deltaTime;
                    transform.ValueRW.Position += movement;
                }
            }
        }
    }
}
