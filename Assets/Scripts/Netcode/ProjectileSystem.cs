using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Collections;

[BurstCompile]
public partial struct ProjectileSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<ProjectileData>();
    }

    public void OnUpdate(ref SystemState state)
    {
        var deltaTime = SystemAPI.Time.DeltaTime;

        // Create an EntityCommandBuffer to store structural changes
        var commandBuffer = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (transform, projectile, entity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<ProjectileData>>().WithEntityAccess())
        {
            if (SystemAPI.HasComponent<LocalTransform>(projectile.ValueRO.Target))
            {
                var targetTransform = SystemAPI.GetComponent<LocalTransform>(projectile.ValueRO.Target);
                float3 targetPosition = targetTransform.Position;

                // Move the projectile towards the target's position
                float3 direction = math.normalize(targetPosition - transform.ValueRW.Position);
                float3 movement = direction * projectile.ValueRO.Speed * deltaTime;
                transform.ValueRW.Position += movement;

                if (math.distance(transform.ValueRW.Position, targetPosition) <= 0.5f)
                {
                    // Log projectile hit
                    UnityEngine.Debug.Log($"Projectile {entity} hit target {projectile.ValueRO.Target}");

                    // Queue up damage application and target destruction in the EntityCommandBuffer
                    ApplyDamageToTarget(ref state, ref commandBuffer, projectile.ValueRO.Target, projectile.ValueRO.Damage);

                    // Queue up projectile destruction in the EntityCommandBuffer
                    commandBuffer.DestroyEntity(entity);
                }
            }
        }

        // Playback the EntityCommandBuffer to apply the queued structural changes
        commandBuffer.Playback(state.EntityManager);
        commandBuffer.Dispose();
    }

    private void ApplyDamageToTarget(ref SystemState state, ref EntityCommandBuffer commandBuffer, Entity target, float damage)
    {
        if (SystemAPI.HasComponent<CombatData>(target))
        {
            var combatData = SystemAPI.GetComponent<CombatData>(target);
            combatData.HitPoints -= damage;

            // If HP falls to zero or below, destroy the target entity using the EntityCommandBuffer
            if (combatData.HitPoints <= 0)
            {
                UnityEngine.Debug.Log($"Target {target} destroyed.");
                commandBuffer.DestroyEntity(target);
            }
            else
            {
                // Update the CombatData component on the target entity
                SystemAPI.SetComponent(target, combatData);
            }
        }
    }
}
