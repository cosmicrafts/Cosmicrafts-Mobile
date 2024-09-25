using Unity.Mathematics;
using Unity.NetCode;
using UnityEditor;

[GhostComponent(PrefabType = GhostPrefabType.AllPredicted)]
public struct PlayerInputData : IInputComponentData
{
    // For 2D movement (x, y)
    public float2 move;
    public InputEvent jump;
}
