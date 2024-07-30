using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.InputSystem;
using Game.Components;

namespace Game.Systems
{
    public class UnitSpawnSystem : MonoBehaviour
    {
        public GameObject unitPrefab;

        private InputAction spawnAction;

        private void OnEnable()
        {
            spawnAction = new InputAction(binding: "<Keyboard>/space");
            spawnAction.performed += ctx => SpawnUnit();
            spawnAction.Enable();
        }

        private void OnDisable()
        {
            spawnAction.Disable();
            spawnAction.performed -= ctx => SpawnUnit();
        }

        private void SpawnUnit()
        {
            Debug.Log("Space key pressed. Attempting to spawn unit...");

            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            var entity = entityManager.CreateEntity();

            // Add components required for rendering
            var renderMeshDescription = new RenderMeshDescription(
                shadowCastingMode: UnityEngine.Rendering.ShadowCastingMode.On,
                receiveShadows: true);

            var renderMeshArray = new RenderMeshArray(
                new Material[] { unitPrefab.GetComponent<MeshRenderer>().sharedMaterial },
                new Mesh[] { unitPrefab.GetComponent<MeshFilter>().sharedMesh });

            RenderMeshUtility.AddComponents(
                entity,
                entityManager,
                renderMeshDescription,
                renderMeshArray,
                MaterialMeshInfo.FromRenderMeshArrayIndices(0, 0));

            entityManager.AddComponentData(entity, new LocalTransform
            {
                Position = float3.zero,
                Rotation = quaternion.identity,
                Scale = 25
            });

            entityManager.AddComponentData(entity, new UnitComponent
            {
                ID = 1,
                PlayerId = 1,
                MyTeam = Team.Blue,
                IsFake = false
            });

            entityManager.AddComponentData(entity, new HealthComponent
            {
                HitPoints = 100,
                MaxHitPoints = 100,
                Shield = 50,
                MaxShield = 50,
                IsDead = false
            });

            // Instantiate the prefab
            GameObject unitGameObject = Instantiate(unitPrefab, Vector3.zero, Quaternion.identity);

            Debug.Log("Unit spawned successfully with following components:");
            Debug.Log($"RenderMesh: Mesh = {unitPrefab.GetComponent<MeshFilter>().sharedMesh.name}, Material = {unitPrefab.GetComponent<MeshRenderer>().sharedMaterial.name}");
            Debug.Log($"LocalTransform: Position = {float3.zero}, Rotation = {quaternion.identity}, Scale = 25");
        }
    }
}