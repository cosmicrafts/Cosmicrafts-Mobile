using UnityEngine;
using TMPro; // Import TextMeshPro namespace

public class Bullet : MonoBehaviour
{
    public GameObject bulletImpactPrefab; // Reference to impact prefab
    public GameObject damageTextPrefab; // Reference to damage text prefab for visual confirmation
    public float impactDestroyDelay = 0.5f; // Time to destroy the impact effect
    public int damage = 1; // Damage the bullet deals
    public float lifespan = 12f; // Time before the bullet is automatically destroyed

    private void Start()
    {
        // Destroy the bullet after its lifespan
        Destroy(gameObject, lifespan);

        // Find the player by tag and ignore collision between bullet and player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Collider2D bulletCollider = GetComponent<Collider2D>();
            Collider2D playerCollider = player.GetComponent<Collider2D>();
            
            // Ignore collision between player and bullet
            if (bulletCollider != null && playerCollider != null)
            {
                Physics2D.IgnoreCollision(bulletCollider, playerCollider);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Prevent the bullet from interacting with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Do nothing if it hits the player
            return;
        }

        // If it hits an asteroid
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            Asteroid asteroid = collision.gameObject.GetComponent<Asteroid>();
            if (asteroid != null)
            {
                // Apply damage to the asteroid
                asteroid.TakeDamage(damage);

                // Display damage as an arc text
                ShowDamageText(collision.transform.position, damage);
            }

            // Instantiate the impact effect
            GameObject impact = Instantiate(bulletImpactPrefab, transform.position, Quaternion.identity);
            Destroy(impact, impactDestroyDelay); // Destroy the impact effect after delay
        }

        // Destroy the bullet on impact with non-player objects
        Destroy(gameObject);
    }

    // Method to instantiate and animate the damage text
    void ShowDamageText(Vector3 position, int damage)
    {
        GameObject damageText = Instantiate(damageTextPrefab, position, Quaternion.identity);
        TMP_Text textMesh = damageText.GetComponent<TMP_Text>();

        if (textMesh != null)
        {
            textMesh.text = damage.ToString(); // Display the damage amount
        }
        else
        {
            Debug.LogError("Missing TMP_Text component on damage text prefab!");
        }

        Destroy(damageText, .25f); // Destroy the damage text after 0.25 seconds
    }
}
