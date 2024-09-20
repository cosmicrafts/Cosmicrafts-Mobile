using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float moveSmoothTime = 0.1f;

    [Header("Dash Settings")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    [Header("Rotation Settings")]
    public float rotationSpeed = 720f;

    [Header("Shooting Settings")]
    public List<Transform> shootPoints; // List of shoot points
    public GameObject bulletPrefab;
    public float bulletSpeed = 20f;
    public float shootingCooldown = 0.1f; // Cooldown for continuous shooting
    public GameObject muzzleFlashPrefab; // Muzzle flash for shooting

    [Header("Thrusters Settings")]
    public List<GameObject> thrusters; // List of thrusters

    [Header("Camera Zoom Settings")]
    public float zoomSmoothSpeed = 5f;  // Smooth transition speed between zoom levels

    // Predefined zoom levels
    private readonly float[] zoomLevels = { 4f, 8f, 16f, 24f, 36f };
    private int currentZoomIndex;  // The current zoom level index

    private Vector2 moveInput;
    private Vector2 smoothMoveVelocity;
    private Vector2 currentMoveSpeed;
    private Vector3 mouseWorldPosition;
    private float zoomInput;

    private bool isDashing;
    private bool isShooting;
    private bool canMove = true; // Disable movement during dash
    private float dashTime;
    private float dashCooldownTimer;
    private float shootingCooldownTimer;

    private Camera mainCamera;
    private Rigidbody2D rb;

    // Input System
    private PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();

        // Movement
        controls.Move.Moveaction.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Move.Moveaction.canceled += ctx => moveInput = Vector2.zero;

        // Dash
        controls.Dash.Dash.performed += ctx => OnDash();

        // Shooting
        controls.Shoot.Shoot.performed += ctx => isShooting = true;
        controls.Shoot.Shoot.canceled += ctx => isShooting = false; // Stop shooting when button is released

        // Mouse Look
        controls.Look.Look.performed += ctx => mouseWorldPosition = GetMouseWorldPosition(ctx);

        // Zoom
        controls.Zoom.Zoom.performed += ctx => zoomInput = ctx.ReadValue<Vector2>().y;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;

        // Initialize the zoom level to the closest matching level to the current orthographic size
        currentZoomIndex = GetClosestZoomIndex(mainCamera.orthographicSize);
    }

    void Update()
    {
        HandleRotation();
        HandleDashTimer();
        HandleShooting();
        UpdateThrusters();
        HandleCameraZoom(); // Handle zooming in/out
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            MoveCharacter();
        }

        rb.angularVelocity = 0f; // Prevent physics-based rotation
    }

    void MoveCharacter()
    {
        if (isDashing) return; // Disable movement during dash

        float speed = moveSpeed;

        Vector2 targetMoveSpeed = moveInput * speed;
        currentMoveSpeed = Vector2.SmoothDamp(rb.linearVelocity, targetMoveSpeed, ref smoothMoveVelocity, moveSmoothTime);
        rb.linearVelocity = currentMoveSpeed;
    }

    Vector3 GetMouseWorldPosition(InputAction.CallbackContext ctx)
    {
        Vector2 mouseScreenPosition = ctx.ReadValue<Vector2>();
        return mainCamera.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, -mainCamera.transform.position.z));
    }

    void HandleRotation()
    {
        if (mouseWorldPosition == Vector3.zero)
            return;

        Vector2 direction = (mouseWorldPosition - transform.position).normalized;

        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        // Apply rotation to the player instantly
        transform.rotation = Quaternion.Euler(0f, 0f, targetAngle);

        rb.MoveRotation(targetAngle); // Ensure Rigidbody2D's rotation matches
    }

    // Handle dash input and timing
    void OnDash()
    {
        if (!isDashing && dashCooldownTimer <= 0f)
        {
            isDashing = true;
            dashTime = dashDuration;
            dashCooldownTimer = dashCooldown;

            // Move in the player's current facing direction (transform.up)
            rb.linearVelocity = transform.up * dashSpeed;
            canMove = false; // Disable movement during dash

            // Activate thrusters during dash
            ActivateThrusters(true);
        }
    }

    void HandleDashTimer()
    {
        if (isDashing)
        {
            dashTime -= Time.deltaTime;
            if (dashTime <= 0f)
            {
                isDashing = false;
                canMove = true; // Re-enable movement after dash

                // Deactivate thrusters after dash ends
                ActivateThrusters(false);
            }
        }

        if (dashCooldownTimer > 0f)
        {
            dashCooldownTimer -= Time.deltaTime;
        }
    }

    // Continuous shooting while holding the button
    void HandleShooting()
    {
        if (isShooting && shootingCooldownTimer <= 0f)
        {
            Shoot();
            shootingCooldownTimer = shootingCooldown;
        }

        if (shootingCooldownTimer > 0f)
        {
            shootingCooldownTimer -= Time.deltaTime;
        }
    }

// Shoot from all shoot points
void Shoot()
{
    foreach (Transform shootPoint in shootPoints)
    {
        // Instantiate the bullet at each shoot point
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        bullet.tag = "Bullet"; // Set the bullet tag
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.linearVelocity = shootPoint.up * bulletSpeed;

        // Show muzzle flash
        if (muzzleFlashPrefab != null)
        {
            GameObject muzzleFlash = Instantiate(muzzleFlashPrefab, shootPoint.position, shootPoint.rotation, shootPoint);
            Destroy(muzzleFlash, 0.1f); // Destroy the muzzle flash after a short duration
        }
    }
}



    // Update thrusters based on movement or dash
    void UpdateThrusters()
    {
        bool isMovingOrDashing = (moveInput != Vector2.zero || isDashing);

        foreach (GameObject thruster in thrusters)
        {
            thruster.SetActive(isMovingOrDashing); // Activate thrusters when moving or dashing
        }
    }

    // Activate or deactivate thrusters
    void ActivateThrusters(bool isActive)
    {
        foreach (GameObject thruster in thrusters)
        {
            thruster.SetActive(isActive);
        }
    }

    // Handle camera zoom based on mouse scroll input
    void HandleCameraZoom()
{
    // Detect if there's a scroll input
    if (zoomInput > 0 && currentZoomIndex > 0)
    {
        // Zoom in, decrease the zoom index
        currentZoomIndex--;
    }
    else if (zoomInput < 0 && currentZoomIndex < zoomLevels.Length - 1)
    {
        // Zoom out, increase the zoom index
        currentZoomIndex++;
    }

    // Clear the zoom input after processing to prevent multiple zoom steps per frame
    zoomInput = 0;

    // Smoothly interpolate the camera's orthographic size to the target zoom level
    float targetZoom = zoomLevels[currentZoomIndex];
    mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetZoom, Time.deltaTime * zoomSmoothSpeed);
}

int GetClosestZoomIndex(float currentZoom)
{
    int closestIndex = 0;
    float closestDifference = Mathf.Abs(currentZoom - zoomLevels[0]);

    for (int i = 1; i < zoomLevels.Length; i++)
    {
        float difference = Mathf.Abs(currentZoom - zoomLevels[i]);
        if (difference < closestDifference)
        {
            closestDifference = difference;
            closestIndex = i;
        }
    }

    return closestIndex;
}

    void FireLaser()
    {
        // Implement laser firing logic here
    }
}
