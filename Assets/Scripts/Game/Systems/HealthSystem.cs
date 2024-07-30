using Unity.Entities;

namespace Game.Systems
{
    public partial class HealthSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((ref Game.Components.HealthComponent health) =>
            {
                if (health.HitPoints < health.MaxHitPoints)
                {
                    health.HitPoints = health.MaxHitPoints; // Example logic
                }
            }).ScheduleParallel();
        }
    }
}
