using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public GameObject explosionPrefab; // Reference to explosion prefab
    public float explosionDestroyDelay = 0.5f; // Time to destroy the explosion effect

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Bullet"))
        {
            // Instantiate the explosion effect and destroy it after a fixed delay
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(explosion, explosionDestroyDelay); // Destroy the explosion effect after delay

            Destroy(gameObject); // Destroy the asteroid
        }
    }
}
