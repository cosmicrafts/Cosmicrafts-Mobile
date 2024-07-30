using Unity.Entities;
using Unity.Rendering;
using UnityEngine;
using Game.Components;

namespace Game.Systems
{
    public partial class SelectionHighlightSystem : SystemBase
    {
        public Material highlightedMaterial;
        public Material originalMaterial;

        protected override void OnUpdate()
        {
            if (highlightedMaterial == null || originalMaterial == null)
            {
                Debug.LogError("Materials are not assigned in SelectionHighlightSystem.");
                return;
            }

            var highlightedMat = highlightedMaterial;
            var originalMat = originalMaterial;

            var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.TempJob);

            Entities
                .WithAll<SelectedComponent>()
                .WithoutBurst()
                .ForEach((Entity entity, in RenderMesh renderMesh) =>
                {
                    var newRenderMesh = renderMesh;
                    newRenderMesh.material = highlightedMat;
                    ecb.SetSharedComponentManaged(entity, newRenderMesh);
                    Debug.Log($"Entity highlighted: {entity}");
                }).Run();

            Entities
                .WithNone<SelectedComponent>()
                .WithoutBurst()
                .ForEach((Entity entity, in RenderMesh renderMesh) =>
                {
                    var newRenderMesh = renderMesh;
                    newRenderMesh.material = originalMat;
                    ecb.SetSharedComponentManaged(entity, newRenderMesh);
                    Debug.Log($"Entity material reset: {entity}");
                }).Run();

            ecb.Playback(EntityManager);
            ecb.Dispose();
        }
    }
}