using UnityEngine;
using Unity.Entities;

public class Unit : MonoBehaviour
{
    public float speed = 3f;
    public float attackRange = 5f;
    public float attackDamage = 10f;
    public float hitPoints = 100f;
}

public struct UnitData : IComponentData
{
    public float speed;
}

public class UnitBaker : Baker<Unit>
{
    public override void Bake(Unit authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent(entity, new UnitData { speed = authoring.speed });
        AddComponent(entity, new CombatData 
        { 
            AttackRange = authoring.attackRange,
            AttackDamage = authoring.attackDamage,
            HitPoints = authoring.hitPoints
        });
        AddComponent<UnitState>(entity);  // Add state management for units
    }
}