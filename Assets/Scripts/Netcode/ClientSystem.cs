using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Mathematics;

public struct ClientMessageRpcCommand : IRpcCommand
{
    public FixedString64Bytes message;
}

[WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
public partial class ClientSystem : SystemBase
{
    protected override void OnCreate()
    {
        RequireForUpdate<NetworkId>();
    }

    protected override void OnUpdate()
    {
        var commandBuffer = new EntityCommandBuffer(Allocator.Temp);

        // Process incoming server messages
        foreach (var (request, command, entity) in SystemAPI.Query<RefRO<ReceiveRpcCommandRequest>, RefRO<ServerMessageRpcCommand>>().WithEntityAccess())
        {
            Debug.Log(command.ValueRO.message);
            commandBuffer.DestroyEntity(entity);
        }

        // Handle unit spawning
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                float3 spawnPosition = hit.point;
                SpawnUnitRpc(ConnectionManager.clientWorld, spawnPosition, 1);
            }
        }

        commandBuffer.Playback(EntityManager);
        commandBuffer.Dispose();
    }

    public void SpawnUnitRpc(World world, float3 spawnPosition, int teamId)
    {
        if (world == null || !world.IsCreated) return;
        var entity = world.EntityManager.CreateEntity(typeof(SendRpcCommandRequest), typeof(SpawnUnitRpcCommand));
        world.EntityManager.SetComponentData(entity, new SpawnUnitRpcCommand
        {
            TeamId = teamId,
            SpawnPosition = spawnPosition
        });
    }
}
