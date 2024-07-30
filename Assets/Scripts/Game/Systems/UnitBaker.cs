// File: Assets/Scripts/Game/Systems/UnitBaker.cs
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Game.Components;

namespace Game.Systems
{
    public class UnitBaker : Baker<UnitAuthoring>
    {
        public override void Bake(UnitAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            // Add the necessary components
            AddComponent(entity, new UnitComponent
            {
                ID = authoring.ID,
                PlayerId = authoring.PlayerId,
                MyTeam = authoring.MyTeam,
                IsFake = authoring.IsFake
            });

            AddComponent(entity, new HealthComponent
            {
                HitPoints = authoring.HitPoints,
                MaxHitPoints = authoring.MaxHitPoints,
                Shield = authoring.Shield,
                MaxShield = authoring.MaxShield,
                IsDead = authoring.IsDead
            });

            AddComponent<LocalTransform>(entity);
        }
    }
}
