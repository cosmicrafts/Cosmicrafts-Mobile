using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Authoring;
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
            spawnAction = new InputAction(binding: "<Mouse>/leftButton");
            spawnAction.performed += ctx => SpawnUnitAtMousePosition();
            spawnAction.Enable();
        }

        private void OnDisable()
        {
            spawnAction.Disable();
            spawnAction.performed -= ctx => SpawnUnitAtMousePosition();
        }

        private void SpawnUnitAtMousePosition()
        {
            // Get the mouse position in the world
            Vector3 mousePosition = Mouse.current.position.ReadValue();
            UnityEngine.Ray ray = Camera.main.ScreenPointToRay(mousePosition);

            if (UnityEngine.Physics.Raycast(ray, out UnityEngine.RaycastHit hit))
            {
                // The point where the mouse clicked
                Vector3 spawnPosition = hit.point;

                var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

                // Create entity
                var entity = entityManager.CreateEntity();

                // Add Transform component
                entityManager.AddComponentData(entity, new LocalTransform
                {
                    Position = new float3(spawnPosition.x, spawnPosition.y, spawnPosition.z),
                    Rotation = quaternion.identity,
                    Scale = 1f
                });

                // Define the box geometry for the collider using Unity.Physics
                var boxGeometry = new Unity.Physics.BoxGeometry
                {
                    Center = float3.zero,
                    Size = new float3(1, 1, 1),
                    Orientation = quaternion.identity
                };

                // Create a BoxCollider using Unity.Physics
                var boxCollider = Unity.Physics.BoxCollider.Create(boxGeometry);

                // Add PhysicsCollider component
                entityManager.AddComponentData(entity, new PhysicsCollider
                {
                    Value = boxCollider
                });

                // Calculate the mass properties from the collider
                var massProperties = boxCollider.Value.MassProperties;

                // Add PhysicsMass component
                entityManager.AddComponentData(entity, PhysicsMass.CreateDynamic(massProperties, 1f));

                // Add Physics components
                entityManager.AddComponentData(entity, new PhysicsVelocity());
                entityManager.AddComponentData(entity, new PhysicsDamping
                {
                    Linear = 0.01f,
                    Angular = 0.05f
                });
                entityManager.AddComponentData(entity, new PhysicsGravityFactor { Value = 0f }); // No gravity in space

                // Add components for rendering
                var renderMeshDescription = new RenderMeshDescription(
                    shadowCastingMode: UnityEngine.Rendering.ShadowCastingMode.On,
                    receiveShadows: true);

                var renderMeshArray = new RenderMeshArray(
                    new UnityEngine.Material[] { unitPrefab.GetComponent<MeshRenderer>().sharedMaterial },
                    new UnityEngine.Mesh[] { unitPrefab.GetComponent<MeshFilter>().sharedMesh });

                RenderMeshUtility.AddComponents(
                    entity,
                    entityManager,
                    renderMeshDescription,
                    renderMeshArray,
                    MaterialMeshInfo.FromRenderMeshArrayIndices(0, 0));

                // Add game-specific components
                entityManager.AddComponentData(entity, new UnitComponent
                {
                    ID = 1,
                    PlayerId = 1,
                    MyTeam = Team.Blue,
                });

                entityManager.AddComponentData(entity, new HealthComponent
                {
                    HitPoints = 100,
                    MaxHitPoints = 100,
                    Shield = 50,
                    MaxShield = 50,
                    IsDead = false
                });

                Debug.Log("Unit spawned successfully at mouse click position with physics.");
            }
        }
    }
}
