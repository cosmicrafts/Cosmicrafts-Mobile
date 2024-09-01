using Unity.Entities;
using Unity.Mathematics;

namespace Game.Systems
{
    public partial class EnergySystem : SystemBase
    {
        protected override void OnUpdate()
        {
            float deltaTime = SystemAPI.Time.DeltaTime;

            Entities.ForEach((ref Game.Components.EnergyComponent energy) =>
            {
                energy.CurrentEnergy = math.min(energy.MaxEnergy, energy.CurrentEnergy + energy.EnergyRegenRate * deltaTime);
            }).ScheduleParallel();
        }
    }
}
