using Unity.Entities;
using UnityEngine;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Physics;
using Unity.Physics.Authoring;

namespace Game
{
    public class UnitBaker
    {
        private EntityManager entityManager;

        public UnitBaker(EntityManager entityManager)
        {
            this.entityManager = entityManager;
        }

        public void Bake(Entity entity, UnitAuthoring authoring)
        {
            Debug.Log("Baking entity: " + entity);

            entityManager.AddComponentData(entity, new Health { Value = authoring.health });
            entityManager.AddComponentData(entity, new Damage { Value = authoring.damage });
            entityManager.AddComponentData(entity, new Team { Value = authoring.team });

            Debug.Log("Added ECS components: Health, Damage, Team");

            // Add LocalTransform component
            entityManager.AddComponentData(entity, new LocalTransform { Position = float3.zero, Rotation = quaternion.identity, Scale = 1f });

            Debug.Log("Added LocalTransform component");

            // Add RenderMeshDescription and RenderMeshArray components for rendering
            var renderMeshDescription = new RenderMeshDescription(
                shadowCastingMode: UnityEngine.Rendering.ShadowCastingMode.On,
                receiveShadows: true);

            var renderMeshArray = new RenderMeshArray(
                new UnityEngine.Material[] { authoring.unitMesh.GetComponent<MeshRenderer>().sharedMaterial },
                new Mesh[] { authoring.unitMesh.GetComponent<MeshFilter>().sharedMesh });

            RenderMeshUtility.AddComponents(
                entity,
                entityManager,
                renderMeshDescription,
                renderMeshArray,
                MaterialMeshInfo.FromRenderMeshArrayIndices(0, 0));

            Debug.Log("Added RenderMesh components");

            // Add built-in Physics components
            var unityCollider = authoring.unitMesh.GetComponent<UnityEngine.Collider>();
            if (unityCollider != null)
            {
                AddColliderComponent(entity, unityCollider);
                Debug.Log("Added Collider component");
            }

            var rigidbody = authoring.unitMesh.GetComponent<UnityEngine.Rigidbody>();
            if (rigidbody != null)
            {
                entityManager.AddComponentObject(entity, rigidbody);
                Debug.Log("Added Rigidbody component");
            }
        }

        private void AddColliderComponent(Entity entity, UnityEngine.Collider unityCollider)
        {
            // Convert UnityEngine.Collider to Unity.Physics.Collider
            if (unityCollider is UnityEngine.BoxCollider boxCollider)
            {
                var boxGeometry = new BoxGeometry
                {
                    Center = boxCollider.center,
                    Size = boxCollider.size,
                    Orientation = quaternion.identity
                };

                var physicsCollider = Unity.Physics.BoxCollider.Create(boxGeometry, new CollisionFilter(), new Unity.Physics.Material());
                entityManager.AddComponentData(entity, new PhysicsCollider { Value = physicsCollider });
            }
            // Add more collider types as needed (SphereCollider, CapsuleCollider, etc.)
        }
    }
}
