using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using Game.Components;

namespace Game.Systems
{
    public partial class UnitMovementSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            if (!Mouse.current.rightButton.wasPressedThisFrame)
                return;

            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                float3 targetPosition = hit.point;

                var translations = GetComponentLookup<LocalTransform>(false);

                Entities.WithAll<SelectedComponent>().WithoutBurst().ForEach((Entity entity) =>
                {
                    if (translations.HasComponent(entity))
                    {
                        var translation = translations[entity];
                        translation.Position = targetPosition;
                        translations[entity] = translation;
                    }
                }).Run();
            }
        }
    }
}
