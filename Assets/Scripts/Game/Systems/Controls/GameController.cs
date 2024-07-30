using Unity.Entities;
using UnityEngine;
using Game.Systems;

public class GameController : MonoBehaviour
{
    public Material highlightedMaterial;
    public Material originalMaterial;

    void Start()
    {
        var world = World.DefaultGameObjectInjectionWorld;
        var system = world.GetExistingSystemManaged<SelectionHighlightSystem>();

        system.highlightedMaterial = highlightedMaterial;
        system.originalMaterial = originalMaterial;
    }
}