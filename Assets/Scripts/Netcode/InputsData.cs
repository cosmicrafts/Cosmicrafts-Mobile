using Unity.Mathematics;
using Unity.NetCode;
using UnityEditor;

[GhostComponent(PrefabType = GhostPrefabType.AllPredicted)]
public struct PlayerInputData : IInputComponentData
{
    //For the values to be sync across server ex move entities
    public float2 move;
    public InputEvent jump;
}