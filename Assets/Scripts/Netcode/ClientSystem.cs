using System.Collections;
using System.Collections.Generic;
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
public struct SpawnUnitRpcCommand : IRpcCommand
{
public float3 position;
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
        
        // Process incoming RPC messages
        foreach (var (request, command, entity) in SystemAPI.Query<RefRO<ReceiveRpcCommandRequest>, RefRO<ServerMessageRpcCommand>>().WithEntityAccess())
        {
            Debug.Log(command.ValueRO.message);
            commandBuffer.DestroyEntity(entity);
        }

        // Detect mouse click and send RPC with the clicked position
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("Mouse clicked!");

            // Project the mouse position onto a plane at a fixed height
            float3 clickedPosition = ProjectMouseToPlane();

            // Send the spawn unit RPC with the clicked position
            SpawnUnitRpc(ConnectionManager.clientWorld, clickedPosition);
        }

        commandBuffer.Playback(EntityManager);
        commandBuffer.Dispose();
    }
    
    private float3 ProjectMouseToPlane()
    {
        // Assume units are always at y = 0 (same height)
        Plane plane = new Plane(Vector3.up, Vector3.zero); // The plane is at height 0
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (plane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter); // Get the hit point on the plane
            return new float3(hitPoint.x, 0, hitPoint.z); // Return the position at y = 0
        }

        // Return a default position if no intersection found (which should not happen)
        return new float3(0f, 0f, 0f);
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

    public void SpawnUnitRpc(World world, float3 position)
    {
        if (world == null || world.IsCreated == false)
        {
            return;
        }

        // Create an entity for sending the RPC command
        var entity = world.EntityManager.CreateEntity(typeof(SendRpcCommandRequest), typeof(SpawnUnitRpcCommand));

        // Set the position in the SpawnUnitRpcCommand
        world.EntityManager.SetComponentData(entity, new SpawnUnitRpcCommand
        {
            position = position  // Send the position to the server
        });

        Debug.Log($"SpawnUnitRpc sent to server with position: {position}");
    }

}
