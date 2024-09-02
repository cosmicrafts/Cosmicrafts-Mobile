using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public struct UnitComponent : IComponentData
{
    public float Speed;
}

[BurstCompile]
public partial struct UnitMovementSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;

        foreach (var (transform, unit, team) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<UnitComponent>, RefRO<TeamComponent>>())
        {
            float3 targetPosition = team.ValueRO.TeamId == 0 ? new float3(20f, 0f, 0f) : new float3(-20f, 0f, 0f);
            float3 direction = math.normalize(targetPosition - transform.ValueRW.Position);
            transform.ValueRW.Position += direction * unit.ValueRO.Speed * deltaTime;
        }
    }
}
