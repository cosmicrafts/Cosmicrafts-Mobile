using UnityEngine;
using UnityEngine.UI; // For the health bar

public class Health : MonoBehaviour
{
    public int maxHealth = 10; // Default health (can be different for asteroids)
    private int currentHealth;

    public GameObject healthBarUI; // Reference to the health bar UI
    public Slider healthSlider; // Slider component for the health bar

    private void Start()
    {
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;

        // Hide the health bar at the start
        healthBarUI.SetActive(false);
    }

    // Method to take damage
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthSlider.value = currentHealth;

        // Show the health bar when the entity takes damage
        if (!healthBarUI.activeSelf)
        {
            healthBarUI.SetActive(true);
        }

        // If health drops to 0, destroy the object
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Method to destroy the object
    void Die()
    {
        if (gameObject.CompareTag("Player"))
        {
            // Implement game over logic here (e.g., disable controls, show game over screen)
        }
        else
        {
            // Destroy the asteroid
            Destroy(gameObject);
        }
    }
}
