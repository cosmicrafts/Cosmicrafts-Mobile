using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Unity.NetCode;

[BurstCompile]
[UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
public partial struct UnitMovementSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<UnitData>();
    }

    [BurstCompile]
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
