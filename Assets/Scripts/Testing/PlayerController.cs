using UnityEngine;
using UnityEngine.InputSystem;

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
    public float rotationSpeed = 720f; // Adjust as needed

    [Header("Shooting Settings")]
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float bulletSpeed = 20f;

    private Vector2 moveInput;
    private Vector2 smoothMoveVelocity;
    private Vector2 currentMoveSpeed;
    private Vector3 mouseWorldPosition;

    private bool isDashing;
    private float dashTime;
    private float dashCooldownTimer;

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
        controls.Shoot.Shoot.performed += ctx => Shoot();

        // Mouse Look (Pointer movement for rotation)
        controls.Look.Look.performed += ctx => mouseWorldPosition = GetMouseWorldPosition(ctx);
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
    }

    void Update()
    {
        HandleRotation();
        HandleDashTimer();
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            rb.linearVelocity = transform.up * dashSpeed;
        }
        else
        {
            MoveCharacter();
        }

        rb.angularVelocity = 0f; // Prevent physics-based rotation
    }

    void MoveCharacter()
    {
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

    void OnDash()
    {
        if (!isDashing && dashCooldownTimer <= 0f)
        {
            isDashing = true;
            dashTime = dashDuration;
            dashCooldownTimer = dashCooldown;
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
            }
        }

        if (dashCooldownTimer > 0f)
        {
            dashCooldownTimer -= Time.deltaTime;
        }
    }

    void Shoot()
    {
        // Use player's current forward direction (transform.up) for shooting direction
        Vector2 shootDirection = transform.up;

        // Instantiate bullet and set its velocity toward where the player is currently facing
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.linearVelocity = shootDirection * bulletSpeed;
    }

    void FireLaser()
    {
        // Implement laser firing logic here
    }
}
