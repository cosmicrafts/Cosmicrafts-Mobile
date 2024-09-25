using UnityEngine;

public class FogOfWarController : MonoBehaviour
{
    public Material fogOfWarMaterial;  // Material using the Fog of War shader
    public Transform playerTransform;  // The player character

    public float revealRadius = 500f;   // Radius of the revealed area

    void Update()
    {
        // Update the shader's player position with the player's current position
        Vector3 playerPos = playerTransform.position;
        fogOfWarMaterial.SetVector("_PlayerPos", new Vector4(playerPos.x, playerPos.y, 0, 0));
        fogOfWarMaterial.SetFloat("_RevealRadius", revealRadius);
    }
}
