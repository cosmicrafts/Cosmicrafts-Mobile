using UnityEngine;
using Unity.Entities;
using Unity.NetCode;
using Unity.Collections;

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
        foreach (RefRW<PlayerInputData> input in SystemAPI.Query<RefRW<PlayerInputData>>().WithAll<GhostOwnerIsLocal>())
        {
            input.ValueRW.move = playerMove;
        }
    }
}
