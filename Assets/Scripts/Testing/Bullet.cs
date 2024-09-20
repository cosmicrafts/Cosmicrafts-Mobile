using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject bulletImpactPrefab; // Reference to impact prefab
    public float impactDestroyDelay = 0.5f; // Time to destroy the impact effect

    public float lifespan = 12f; // Time before the bullet is automatically destroyed

    private void Start()
    {
        // Destroy the bullet after its lifespan
        Destroy(gameObject, lifespan);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            // Instantiate the impact effect and destroy it after a fixed delay
            GameObject impact = Instantiate(bulletImpactPrefab, transform.position, Quaternion.identity);
            Destroy(impact, impactDestroyDelay); // Destroy the impact effect after delay

            Destroy(gameObject); // Destroy the bullet
        }
    }
}
