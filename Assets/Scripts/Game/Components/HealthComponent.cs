// File: Assets/Scripts/Game/Components/HealthComponent.cs
using Unity.Entities;

namespace Game.Components
{
    public struct HealthComponent : IComponentData
    {
        public int HitPoints;
        public int MaxHitPoints;
        public int Shield;
        public int MaxShield;
        public bool IsDead;
    }
}
    