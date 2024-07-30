using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.InputSystem;
using Game.Components;

namespace Game.Systems
{
    public class UnitSelectionSystem : MonoBehaviour
    {
        public Camera mainCamera;
        public LayerMask selectableLayer;

        void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, selectableLayer))
                {
                    Entity entity = GetEntityFromCollider(hit.collider);
                    if (entity != Entity.Null)
                    {
                        Debug.Log($"Entity selected: {entity}");
                        SelectEntity(entity);
                    }
                    else
                    {
                        Debug.Log("No entity found on collider.");
                    }
                }
                else
                {
                    Debug.Log("Raycast did not hit any object in the selectable layer.");
                }
            }
        }

        private Entity GetEntityFromCollider(Collider collider)
        {
            var entityReference = collider.GetComponent<EntityReference>();
            Debug.Log("No EntityReference found on collider.");
            return Entity.Null;
        }

        private void SelectEntity(Entity entity)
        {
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            if (entityManager.HasComponent<SelectedComponent>(entity))
            {
                entityManager.RemoveComponent<SelectedComponent>(entity);
                Debug.Log($"Deselected entity: {entity}");
            }
            else
            {
                entityManager.AddComponent<SelectedComponent>(entity);
                Debug.Log($"Selected entity: {entity}");
            }
        }
    }
}
