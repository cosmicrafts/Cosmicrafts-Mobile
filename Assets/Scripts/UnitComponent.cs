using Unity.Entities;
using Unity.Mathematics;

namespace Game
{
    public struct Health : IComponentData
    {
        public int Value;
    }

    public struct Damage : IComponentData
    {
        public int Value;
    }

    public struct Team : IComponentData
    {
        public int Value;
    }

    public struct UnitMesh : IComponentData
    {
        public Entity Value;
    }

    public struct UnitPrefab : IComponentData
    {
        public Entity Value;
    }

    public struct SpawnPoint : IComponentData
    {
        public float3 Value;
    }

    public struct TargetPosition : IComponentData
    {
        public float3 Value;
    }
}
