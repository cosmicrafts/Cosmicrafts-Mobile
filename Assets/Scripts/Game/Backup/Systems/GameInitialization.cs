using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Game.Systems
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial class GameInitializationSystem : SystemBase
    {
        protected override void OnCreate()
        {
            // Set up initial game state
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            // Create Player Entity
            Entity playerEntity = entityManager.CreateEntity(
                typeof(Game.Components.PlayerComponent),
                typeof(Game.Components.HealthComponent)
            );

            entityManager.SetComponentData(playerEntity, new Game.Components.PlayerComponent
            {
                ID = 1,
                MyTeam = Game.Components.Team.Blue,
                CurrentEnergy = 5,
                MaxEnergy = 10,
                SpeedEnergy = 1
            });

            entityManager.SetComponentData(playerEntity, new Game.Components.HealthComponent
            {
                HitPoints = 100,
                MaxHitPoints = 100,
                Shield = 50,
                MaxShield = 50,
                IsDead = false
            });

            // More initialization logic if needed
        }

        protected override void OnUpdate()
        {
            // Initialization logic
        }
    }
}
