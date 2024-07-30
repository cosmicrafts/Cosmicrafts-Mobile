using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game
{
    public class GameBootstrap : MonoBehaviour
    {
        public GameObject unitPrefab;
        public GameObject spawnPoint;

        void Start()
        {
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            // Convert the GameObject to an entity using the Baker
            Entity unitPrefabEntity = ConvertToEntity(unitPrefab);

            // Create a singleton entity with the UnitPrefab component
            var singletonEntity = entityManager.CreateEntity();
            entityManager.AddComponentData(singletonEntity, new UnitPrefab { Value = unitPrefabEntity });

            // Create a singleton entity with the SpawnPoint component
            var spawnPointEntity = entityManager.CreateEntity();
            entityManager.AddComponentData(spawnPointEntity, new SpawnPoint { Value = (float3)spawnPoint.transform.position });
        }

        private Entity ConvertToEntity(GameObject gameObject)
        {
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            // Simulate the baking process
            var unitAuthoring = gameObject.GetComponent<UnitAuthoring>();
            if (unitAuthoring != null)
            {
                var entity = entityManager.CreateEntity();
                var baker = new UnitBaker(entityManager);
                baker.Bake(entity, unitAuthoring);
                Debug.Log("Baking completed for unitPrefab.");
                return entity;
            }
            return Entity.Null;
        }
    }
}
