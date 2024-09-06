using Unity.Entities;
using Unity.NetCode;
using UnityEngine;
using UnityEngine.InputSystem;

[WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
public partial class TeamSelectionSystem : SystemBase
{
    protected override void OnCreate()
    {
        RequireForUpdate<NetworkId>();
    }

    protected override void OnUpdate()
    {
        // This is where you would trigger the UI for team selection.
        // For simplicity, let's use digit keys to select a team
        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            SendTeamSelection(1); // Team A
            Debug.Log("Team A selected");
        }
        else if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            SendTeamSelection(2); // Team B
            Debug.Log("Team B selected");
        }
    }

    private void SendTeamSelection(int teamId)
    {
        var world = ConnectionManager.clientWorld;
        if (world == null || !world.IsCreated) return;

        var entity = world.EntityManager.CreateEntity(typeof(SendRpcCommandRequest), typeof(TeamSelectionRpcCommand));
        world.EntityManager.SetComponentData(entity, new TeamSelectionRpcCommand
        {
            TeamId = teamId
        });
    }
}
