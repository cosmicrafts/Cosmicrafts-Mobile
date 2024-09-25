using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float speed = 2f;
    public float rotationSpeed = 360f; // Speed at which the enemy rotates toward the player
    private Transform player;
    private Rigidbody2D rb;

    private void Start()
    {
        player = Camera.main.transform; // Assume player is the main camera
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (player != null)
        {
            // Get direction to the player
            Vector2 direction = (player.position - transform.position).normalized;

            // Rotate smoothly towards the player
            RotateTowardsPlayer(direction);

            // Move towards the player
            MoveTowardsPlayer(direction);

            // Prevent physics-based rotation from affecting the enemy
            rb.angularVelocity = 0f;
        }
    }

    // Rotate the enemy towards the player
    private void RotateTowardsPlayer(Vector2 direction)
    {
        // Calculate the angle between the enemy's current direction and the player
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        // Smoothly rotate the enemy to face the player
        float smoothedAngle = Mathf.LerpAngle(rb.rotation, targetAngle, rotationSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(smoothedAngle);
    }

    // Move the enemy towards the player
    private void MoveTowardsPlayer(Vector2 direction)
    {
        // Move directly toward the player by setting the velocity
        rb.linearVelocity = direction * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // You can handle additional collision behavior here if needed.
        // For example, reduce health if the enemy collides with certain objects.
    }
}
