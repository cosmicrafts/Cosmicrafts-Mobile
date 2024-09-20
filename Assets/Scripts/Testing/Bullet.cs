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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If it hits an asteroid
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            // Get the asteroid component
            Asteroid asteroid = collision.gameObject.GetComponent<Asteroid>();
            if (asteroid != null)
            {
                // Apply damage to the asteroid
                asteroid.TakeDamage(damage);

                // Display damage as an arc text
                ShowDamageText(collision.transform.position, damage);
            }

            // Instantiate the impact effect regardless of whether the asteroid was destroyed
            GameObject impact = Instantiate(bulletImpactPrefab, transform.position, Quaternion.identity);
            Destroy(impact, impactDestroyDelay); // Destroy the impact effect after delay
        }

        // Destroy the bullet on impact with anything (even if asteroid is not destroyed)
        Destroy(gameObject);
    }

    // Method to instantiate and animate the damage text
    void ShowDamageText(Vector3 position, int damage)
    {
        // Instantiate the damage text at the collision position
        GameObject damageText = Instantiate(damageTextPrefab, position, Quaternion.identity);

        // Get the TMP component from the damage text prefab
        TMP_Text textMesh = damageText.GetComponent<TMP_Text>();
        
        // Check if the TMP_Text component exists
        if (textMesh != null)
        {
            textMesh.text = damage.ToString(); // Display the damage amount
        }
        else
        {
            Debug.LogError("Missing TMP_Text component on damage text prefab!");
        }

        // Optionally, add animation to move the text upwards in an arc
        Destroy(damageText, .15f); // Destroy the damage text after 1 second
    }
}
