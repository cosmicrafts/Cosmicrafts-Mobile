using UnityEngine;
using Unity.Entities;

public class Unit : MonoBehaviour
{
    public float speed = 3f; // Set a default speed or tweak from the Inspector
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
        AddComponent(entity, new UnitData
        {
            speed = authoring.speed
        });
    }
}