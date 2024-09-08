using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;
using Unity.Mathematics;
using Unity.Transforms;

public struct ServerMessageRpcCommand : IRpcCommand
{
    public FixedString64Bytes message;
}

public struct InitializedClient : IComponentData
{

}
public struct Team : IComponentData
{
    public int Value;
}


[WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
public partial class ServerSystem : SystemBase
{
    private ComponentLookup<NetworkId> _clients;
    protected override void OnCreate()
    {
        _clients = GetComponentLookup<NetworkId>(true);
    }
    protected override void OnUpdate()
    {
        _clients.Update(this);
        var commandBuffer = new EntityCommandBuffer(Allocator.Temp);

        
        foreach (var (request, command, entity) in SystemAPI.Query<RefRO<ReceiveRpcCommandRequest>, RefRO<ClientMessageRpcCommand>>().WithEntityAccess())
        {
            Debug.Log(command.ValueRO.message + " from client index " + request.ValueRO.SourceConnection.Index + " version " + request.ValueRO.SourceConnection.Version);
            commandBuffer.DestroyEntity(entity);
        }

        // Spawn Player and Assign Team
        foreach (var (id, entity) in SystemAPI.Query<RefRO<NetworkId>>().WithNone<InitializedClient>().WithEntityAccess())
        {
            commandBuffer.AddComponent<InitializedClient>(entity);

            // Assign random team (e.g., 0 or 1 for two teams)
            int assignedTeam = UnityEngine.Random.Range(0, 2);
            commandBuffer.AddComponent(entity, new Team { Value = assignedTeam });

            PrefabsData prefabManager = SystemAPI.GetSingleton<PrefabsData>();
            if (prefabManager.player != null)
            {
                Entity player = commandBuffer.Instantiate(prefabManager.player);
                commandBuffer.SetComponent(player, new LocalTransform()
                {
                    Position = new float3(UnityEngine.Random.Range(-10f, 10f), 1, UnityEngine.Random.Range(-10f, 10f)),
                    Rotation = quaternion.identity,
                    Scale = 1f
                });

                // Assign the same team to the player
                commandBuffer.AddComponent(player, new Team { Value = assignedTeam });

                // Assign the network ID
                commandBuffer.SetComponent(player, new GhostOwner()
                {
                    NetworkId = id.ValueRO.Value
                });

                // Link to connection
                commandBuffer.AppendToBuffer(entity, new LinkedEntityGroup()
                {
                    Value = player
                });
            }
        }

        // Spawn Units with Team Assignment
        foreach (var (request, command, entity) in SystemAPI.Query<RefRO<ReceiveRpcCommandRequest>, RefRO<SpawnUnitRpcCommand>>().WithEntityAccess())
        {
            PrefabsData prefabs;
            if (SystemAPI.TryGetSingleton<PrefabsData>(out prefabs) && prefabs.unit != null)
            {
                Entity unit = commandBuffer.Instantiate(prefabs.unit);

                // Get the clicked position from the command
                float3 clickedPosition = command.ValueRO.position;
                commandBuffer.SetComponent(unit, new LocalTransform()
                {
                    Position = clickedPosition,
                    Rotation = quaternion.identity,
                    Scale = 1f
                });

                // Get the team from the connection and assign to the unit using SystemAPI
                var networkId = _clients[request.ValueRO.SourceConnection];
                var connectionTeam = SystemAPI.GetComponent<Team>(request.ValueRO.SourceConnection);
                commandBuffer.AddComponent(unit, new Team { Value = connectionTeam.Value });

                // Assign the owner
                commandBuffer.SetComponent(unit, new GhostOwner()
                {
                    NetworkId = networkId.Value
                });

                // Link unit to connection
                commandBuffer.AppendToBuffer(request.ValueRO.SourceConnection, new LinkedEntityGroup()
                {
                    Value = unit
                });

                commandBuffer.DestroyEntity(entity);
            }
        }
        commandBuffer.Playback(EntityManager);
        commandBuffer.Dispose();
    }

    public void SendMessageRpc(string text, World world, Entity target = default)
    {
        if (world == null || world.IsCreated == false)
        {
            return;
        }
        var entity = world.EntityManager.CreateEntity(typeof(SendRpcCommandRequest), typeof(ServerMessageRpcCommand));
        world.EntityManager.SetComponentData(entity, new ServerMessageRpcCommand()
        {
            message = text
        });
        if (target != Entity.Null)
        {
            world.EntityManager.SetComponentData(entity, new SendRpcCommandRequest()
            {
                TargetConnection = target
            });
        }
    }

}