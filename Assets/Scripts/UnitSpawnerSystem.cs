using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine.InputSystem;

namespace Game
{
    public partial class UnitSpawnSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

                // Retrieve the unit prefab entity from the singleton component
                Entity unitPrefabEntity = SystemAPI.GetSingleton<UnitPrefab>().Value;
                float3 spawnPoint = SystemAPI.GetSingleton<SpawnPoint>().Value;

                // Instantiate a new unit from the prefab
                Entity newUnit = entityManager.Instantiate(unitPrefabEntity);
                entityManager.SetComponentData(newUnit, new LocalTransform { Position = spawnPoint, Rotation = quaternion.identity, Scale = 1f });
            }
        }
    }
}
