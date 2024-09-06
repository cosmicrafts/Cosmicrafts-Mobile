using Unity.Entities;
using Unity.NetCode;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;
using UnityEngine;

[WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
public partial class TeamAssignmentSystem : SystemBase
{
    private ComponentLookup<NetworkId> _clients;

    protected override void OnCreate()
    {
        _clients = GetComponentLookup<NetworkId>(true);
        RequireForUpdate<TeamSelectionRpcCommand>();
    }

    protected override void OnUpdate()
    {
        _clients.Update(this);
        var commandBuffer = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (request, command, entity) in SystemAPI.Query<RefRO<ReceiveRpcCommandRequest>, RefRO<TeamSelectionRpcCommand>>().WithEntityAccess())
        {
            var teamId = command.ValueRO.TeamId;
            var networkId = _clients[request.ValueRO.SourceConnection];

            // Spawn the player now based on the team selection
            Entity playerEntity = SpawnPlayerForConnection(request.ValueRO.SourceConnection, commandBuffer, teamId, networkId);
            if (playerEntity != Entity.Null)
            {
                // Assign the player to the selected team
                commandBuffer.AddComponent(playerEntity, new TeamComponent { TeamId = teamId });

                // Attach movement-related components
                commandBuffer.AddComponent(playerEntity, new PlayerData { speed = 5f }); // Example movement component
                commandBuffer.AddComponent<PlayerInputData>(playerEntity); // Attach input component

                // Log the team assignment
                Debug.Log($"Player with NetworkId {networkId.Value} assigned to Team {teamId}");
            }

            // Properly consume and destroy the RPC entity to prevent warnings
            commandBuffer.DestroyEntity(entity);
        }

        commandBuffer.Playback(EntityManager);
        commandBuffer.Dispose();
    }

    private Entity SpawnPlayerForConnection(Entity connectionEntity, EntityCommandBuffer commandBuffer, int teamId, NetworkId networkId)
    {
        PrefabsData prefabManager = SystemAPI.GetSingleton<PrefabsData>();
        if (prefabManager.player != null)
        {
            Entity player = commandBuffer.Instantiate(prefabManager.player);

            // Assign ownership of the player entity to the client
            commandBuffer.SetComponent(player, new GhostOwner { NetworkId = networkId.Value });

            // Link player entity to the connection entity
            commandBuffer.AddBuffer<LinkedEntityGroup>(connectionEntity).Add(new LinkedEntityGroup { Value = player });

            // Set initial transform based on team selection
            float3 spawnPosition = teamId == 1 ? new float3(-10, 0, 0) : new float3(10, 0, 0);
            commandBuffer.SetComponent(player, new LocalTransform
            {
                Position = spawnPosition,
                Rotation = quaternion.identity,
                Scale = 1f
            });

            return player;
        }
        return Entity.Null;
    }
}
