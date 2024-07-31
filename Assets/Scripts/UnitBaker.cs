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

            // Add Physics components
            AddPhysicsComponents(entity, authoring);
        }

        private void AddPhysicsComponents(Entity entity, UnitAuthoring authoring)
        {
            var boxCollider = authoring.unitMesh.GetComponent<UnityEngine.BoxCollider>();
            if (boxCollider != null)
            {
                AddBoxCollider(entity, boxCollider);
                Debug.Log("Added BoxCollider component");
            }

            var rigidbody = authoring.unitMesh.GetComponent<UnityEngine.Rigidbody>();
            if (rigidbody != null)
            {
                AddRigidbodyComponent(entity, rigidbody);
                Debug.Log("Added Rigidbody component");
            }
        }

        private void AddBoxCollider(Entity entity, UnityEngine.BoxCollider boxCollider)
        {
            var colliderGeometry = new BoxGeometry
            {
                Center = boxCollider.center,
                Size = boxCollider.size * 2f, // Making the collider larger for testing
                Orientation = quaternion.identity
            };

            var collisionFilter = new CollisionFilter
            {
                BelongsTo = ~0u,
                CollidesWith = ~0u,
                GroupIndex = 0
            };

            var physicsCollider = Unity.Physics.BoxCollider.Create(colliderGeometry, collisionFilter, new Unity.Physics.Material
            {
                Friction = 0.5f,
                Restitution = 0.1f
            });

            entityManager.AddComponentData(entity, new PhysicsCollider { Value = physicsCollider });
        }

        private void AddRigidbodyComponent(Entity entity, UnityEngine.Rigidbody unityRigidbody)
        {
            var massProperties = new MassProperties
            {
                MassDistribution = new MassDistribution
                {
                    Transform = RigidTransform.identity,
                    InertiaTensor = new float3(1f, 1f, 1f)
                },
                Volume = 1f,
                AngularExpansionFactor = 1f
            };

            var physicsMass = PhysicsMass.CreateDynamic(massProperties, unityRigidbody.mass);
            var physicsVelocity = new PhysicsVelocity
            {
                Linear = float3.zero,
                Angular = float3.zero
            };

            var physicsDamping = new PhysicsDamping
            {
                Linear = unityRigidbody.linearDamping,
                Angular = unityRigidbody.angularDamping
            };

            entityManager.AddComponentData(entity, physicsMass);
            entityManager.AddComponentData(entity, physicsVelocity);
            entityManager.AddComponentData(entity, physicsDamping);
            entityManager.AddComponentData(entity, new PhysicsGravityFactor { Value = 1.0f });
        }
    }
}
