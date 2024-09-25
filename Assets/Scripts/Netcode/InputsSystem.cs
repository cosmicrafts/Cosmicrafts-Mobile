using UnityEngine;
using Unity.Entities;
using Unity.NetCode;
using Unity.Collections;
using Unity.Mathematics;

[UpdateInGroup(typeof(GhostInputSystemGroup))]
[WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
public partial class InputsSystem : SystemBase
{
    private PlayerControls _controls;

    protected override void OnCreate()
    {
        _controls = new PlayerControls();
        _controls.Enable();

        var builder = new EntityQueryBuilder(Allocator.Temp);
        builder.WithAny<PlayerInputData>();
        RequireForUpdate(GetEntityQuery(builder));
    }

    protected override void OnDestroy()
    {
        _controls.Disable();
    }

    protected override void OnUpdate()
    {
        // Get player movement inputs from the PlayerControls.inputactions
        Vector2 playerMove = _controls.Move.Moveaction.ReadValue<Vector2>();

        // Apply player movement to the PlayerInputData
        foreach (var (input, transform, targetPosition) in SystemAPI.Query<RefRW<PlayerInputData>, RefRO<LocalTransform>, RefRW<TargetPosition>>().WithAll<GhostOwnerIsLocal>())
        {
            // Update the PlayerInputData to track the current movement
            input.ValueRW.move = new float2(playerMove.x, playerMove.y);

            // Update the target position based on the player’s current position and input
            float2 currentPosition = transform.ValueRO.Position.xy;
            float2 newTargetPosition = currentPosition + input.ValueRW.move; // Moves to new direction
            
            // Set the new target position for movement
            targetPosition.ValueRW.Value = newTargetPosition;
        }
    }
}
