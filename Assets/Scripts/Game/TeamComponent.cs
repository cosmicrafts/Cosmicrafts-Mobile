using Unity.Entities;
using Unity.NetCode;

    public struct TeamComponent : IComponentData
    {
        public int TeamId; // 1 for Team A, 2 for Team B
    }
    public struct TeamSelectionRpcCommand : IRpcCommand
    {
        public int TeamId; // 1 for Team A, 2 for Team B
    }