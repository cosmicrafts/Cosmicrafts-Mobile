using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;
using UnityEngine.InputSystem;

public struct ClientMessageRpcCommand : IRpcCommand
{
    public FixedString64Bytes message;
}
public struct SpawnUnitRpcCommand : IRpcCommand
{
    // assign different variables
    // type of unit or properties for the unit
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
        foreach (var (request, command, entity) in SystemAPI.Query<RefRO<ReceiveRpcCommandRequest>, RefRO<ServerMessageRpcCommand>>().WithEntityAccess())
        {
            Debug.Log(command.ValueRO.message);
            commandBuffer.DestroyEntity(entity);
        }

        // Use the new Input System
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            SendMessageRpc("Hello", ConnectionManager.ClientWorld);
        }
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            SpawnUnitRpc(ConnectionManager.ClientWorld);
        }

        commandBuffer.Playback(EntityManager);
        commandBuffer.Dispose();
    }

    public void SendMessageRpc(string text, World world)
    {
        if (world == null || world.IsCreated == false)
        {
            return;
        }
        var entity = world.EntityManager.CreateEntity(typeof(SendRpcCommandRequest), typeof(ClientMessageRpcCommand));
        world.EntityManager.SetComponentData(entity, new ClientMessageRpcCommand()
        {
            message = text
        });
    }

    public void SpawnUnitRpc(World world)
    {
        if (world == null || world.IsCreated == false)
        {
            return;
        }
        world.EntityManager.CreateEntity(typeof(SendRpcCommandRequest), typeof(SpawnUnitRpcCommand));
    }
}
