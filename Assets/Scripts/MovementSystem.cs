using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine; // For Debug.Log

namespace Game
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public partial class UnitMovementSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            float deltaTime = SystemAPI.Time.DeltaTime;

            Entities
                .WithAll<TargetPosition>()
                .ForEach((ref LocalTransform localTransform, in TargetPosition targetPosition) =>
                {
                    float3 direction = math.normalize(targetPosition.Value - localTransform.Position);
                    localTransform.Position += direction * deltaTime; // Adjust speed as necessary

                    // Debug logging to verify movement logic
                    Debug.Log($"Entity moving to {targetPosition.Value}. Current position: {localTransform.Position}");
                }).ScheduleParallel();
        }
    }
}

public struct TargetPosition : IComponentData
{
    public float3 Value;
}
