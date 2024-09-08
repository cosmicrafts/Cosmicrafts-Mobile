using UnityEngine;
using Unity.Entities;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    public float attackRange = 7f;
    public float detectionRange = 12f;  // Add detection range
    public float attackDamage = 15f;
    public float hitPoints = 150f;
}

public struct PlayerData : IComponentData
{
    public float speed;
}

public class PlayerBaker : Baker<Player>
{
    public override void Bake(Player authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent(entity, new PlayerData { speed = authoring.speed });
        AddComponent(entity, new CombatData 
        { 
            AttackRange = authoring.attackRange,
            DetectionRange = authoring.detectionRange,  // Set detection range
            AttackDamage = authoring.attackDamage,
            HitPoints = authoring.hitPoints
        });
        AddComponent<UnitState>(entity);  // Add state management for players
        AddComponent<PlayerInputData>(entity);  // Ensure input is captured for player entities
        AddComponent(entity, new Team { Value = 0 });
    }
}
