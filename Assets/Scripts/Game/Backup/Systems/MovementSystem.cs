using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;
using Game.Components;

namespace Game.Systems
{
    public partial class MovementSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            float deltaTime = SystemAPI.Time.DeltaTime;
            float moveSpeed = 2f;

            Entities.ForEach((ref LocalTransform localTransform, ref UnitComponent unit) =>
            {
                if (unit.MyTeam == Team.Blue) // Example condition
                {
                    localTransform.Position += new float3(0, 0, moveSpeed * deltaTime);
                }
            }).Run();
        }
    }
}
