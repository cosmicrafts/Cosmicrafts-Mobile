using UnityEngine;


public class Asteroid : MonoBehaviour

{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with the player or any object that should destroy the asteroid
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject); // Destroy the asteroid on impact
        }
    }
}
