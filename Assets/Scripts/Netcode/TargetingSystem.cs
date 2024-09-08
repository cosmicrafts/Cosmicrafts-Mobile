using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Collections;

[BurstCompile]
public partial struct TargetingSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<CombatData>();
    }

    public void OnUpdate(ref SystemState state)
    {
        var time = SystemAPI.Time.ElapsedTime;

        // Create an EntityCommandBuffer to queue up structural changes
        var commandBuffer = new EntityCommandBuffer(Allocator.Temp);

        // Iterate over units or bases that have CombatData and belong to a team
        foreach (var (transform, combat, team, unitState, entity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<CombatData>, RefRO<Team>, RefRW<UnitState>>().WithEntityAccess())
        {
            // Find enemy units or bases in range
            foreach (var (enemyTransform, enemyTeam, enemyEntity) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<Team>>().WithEntityAccess())
            {
                // Skip units on the same team
                if (team.ValueRO.Value == enemyTeam.ValueRO.Value)
                    continue;

                // Check if the enemy is within attack range
                float distance = math.distance(transform.ValueRW.Position, enemyTransform.ValueRO.Position);
                if (distance <= combat.ValueRW.AttackRange && (time - combat.ValueRW.LastAttackTime) >= combat.ValueRW.AttackCooldown)
                {
                    // Log attack start
                    UnityEngine.Debug.Log($"Unit {entity} attacking target {enemyEntity}");

                    // Spawn the projectile targeting the enemy entity
                    SpawnProjectile(ref state, ref commandBuffer, transform.ValueRW.Position, enemyEntity, combat.ValueRW.AttackDamage);

                    // Update last attack time
                    combat.ValueRW.LastAttackTime = (float)time;

                    // Set unit to attacking state
                    unitState.ValueRW.IsAttacking = true;

                    // Log attack finish
                    UnityEngine.Debug.Log($"Projectile spawned from {entity} targeting {enemyEntity}");

                    break; // Stop after attacking one enemy in range
                }
            }
        }

        // Apply the structural changes after the iteration is complete
        commandBuffer.Playback(state.EntityManager);
        commandBuffer.Dispose();
    }

    // SpawnProjectile function that launches a projectile towards the target entity
    private void SpawnProjectile(ref SystemState state, ref EntityCommandBuffer commandBuffer, float3 startPosition, Entity targetEntity, float damage)
    {
        var projectileEntity = commandBuffer.CreateEntity();

        // Set projectile data with the target entity reference
        commandBuffer.AddComponent(projectileEntity, new ProjectileData
        {
            Damage = damage,
            Speed = 10f,  // Example speed
            Target = targetEntity // Use the enemy entity as the target
        });

        // Set the projectile's starting position and orientation
        commandBuffer.AddComponent(projectileEntity, new LocalTransform
        {
            Position = startPosition,
            Rotation = quaternion.identity,
            Scale = 1f
        });
    }
}
