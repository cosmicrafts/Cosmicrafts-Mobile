using Unity.Entities;

public struct TeamComponent : IComponentData
{
    public int TeamId; // 0 for Team A, 1 for Team B
}