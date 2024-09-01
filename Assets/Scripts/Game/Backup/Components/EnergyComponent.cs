using Unity.Entities;

namespace Game.Components
{
    public struct EnergyComponent : IComponentData
    {
        public float CurrentEnergy;
        public float MaxEnergy;
        public float EnergyRegenRate;
    }
}
