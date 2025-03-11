using System;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Transform playerCam;
    public Transform orientation;

    private Rigidbody rb;

    private float xRotation;
    private float sensitivity = 50f;
    private float sensMultiplier = 1f;

    public float moveSpeed = 4500;
    public float maxSpeed = 20;
    public bool grounded;
    public LayerMask whatIsGround;

    public float counterMovement = 0.175f;
    private float threshold = 0.01f;
    public float maxSlopeAngle = 35f;

    private Vector3 normalVector = Vector3.up;

    private bool readyToJump = true;
    private float jumpCooldown = 0.25f;
    public float jumpForce = 550f;

    float x, y;
    bool jumping;

    public float dashSpeed = 30000f; 
    private bool isDashing = false;
    private float dashTime = 0.3f; 
    private float dashTimeRemaining;
    private float dashCooldown = 3f; 
    private float dashCooldownRemaining = 0f;

    public float dashFOV = 90; 
    private float normalFOV = 60f; 
    private float FOVChangeSpeed = 10f; 

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        normalFOV = playerCam.GetComponent<Camera>().fieldOfView;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Update()
    {
        MyInput();
        Look();
        UpdateCameraFOV();
    }

    private void MyInput()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        jumping = Input.GetButton("Jump");

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && dashCooldownRemaining <= 0)
        {
            StartDash();
        }
    }

    private void Move()
    {
        rb.AddForce(Vector3.down * Time.deltaTime * 10);

        Vector2 mag = FindVelRelativeToLook();
        float xMag = mag.x, yMag = mag.y;

        CounterMovement(x, y, mag);

        if (readyToJump && jumping) Jump();

        float maxSpeed = this.maxSpeed;

        if (x > 0 && xMag > maxSpeed) x = 0;
        if (x < 0 && xMag < -maxSpeed) x = 0;
        if (y > 0 && yMag > maxSpeed) y = 0;
        if (y < 0 && yMag < -maxSpeed) y = 0;

        float multiplier = grounded ? 1f : 0.5f;

        if (isDashing)
        {
            rb.AddForce(orientation.transform.forward * y * dashSpeed * Time.deltaTime * multiplier);
            dashTimeRemaining -= Time.deltaTime;
            if (dashTimeRemaining <= 0)
            {
                StopDash();
            }
        }
        else
        {
            rb.AddForce(orientation.transform.forward * y * moveSpeed * Time.deltaTime * multiplier);
            rb.AddForce(orientation.transform.right * x * moveSpeed * Time.deltaTime * multiplier);
        }

        if (dashCooldownRemaining > 0)
        {
            dashCooldownRemaining -= Time.deltaTime;
        }
    }

    private void StartDash()
    {
        isDashing = true;
        dashTimeRemaining = dashTime;
        dashCooldownRemaining = dashCooldown;
    }

    private void StopDash()
    {
        isDashing = false;
    }

    private void Jump()
    {
        if (grounded && readyToJump)
        {
            readyToJump = false;
            rb.AddForce(Vector2.up * jumpForce * 1.5f);
            rb.AddForce(normalVector * jumpForce * 0.5f);

            Vector3 vel = rb.velocity;
            rb.velocity = new Vector3(vel.x, Mathf.Max(vel.y / 2, 0), vel.z);

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime * sensMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime * sensMultiplier;

        Vector3 rot = playerCam.transform.localRotation.eulerAngles;
        float desiredX = rot.y + mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, 0);
        orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);
    }

    private void CounterMovement(float x, float y, Vector2 mag)
    {
        if (!grounded || jumping) return;

        if (Math.Abs(mag.x) > threshold && Math.Abs(x) < 0.05f || (mag.x < -threshold && x > 0) || (mag.x > threshold && x < 0))
        {
            rb.AddForce(moveSpeed * orientation.transform.right * Time.deltaTime * -mag.x * counterMovement);
        }
        if (Math.Abs(mag.y) > threshold && Math.Abs(y) < 0.05f || (mag.y < -threshold && y > 0) || (mag.y > threshold && y < 0))
        {
            rb.AddForce(moveSpeed * orientation.transform.forward * Time.deltaTime * -mag.y * counterMovement);
        }
    }

    public Vector2 FindVelRelativeToLook()
    {
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitude = rb.velocity.magnitude;
        float yMag = magnitude * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitude * Mathf.Cos(v * Mathf.Deg2Rad);

        return new Vector2(xMag, yMag);
    }

    private void OnCollisionStay(Collision other)
    {
        int layer = other.gameObject.layer;
        if (whatIsGround != (whatIsGround | (1 << layer))) return;

        for (int i = 0; i < other.contactCount; i++)
        {
            Vector3 normal = other.contacts[i].normal;
            if (Vector3.Angle(Vector3.up, normal) < maxSlopeAngle)
            {
                grounded = true;
                normalVector = normal;
                CancelInvoke(nameof(StopGrounded));
                StopDash();
            }
        }

        Invoke(nameof(StopGrounded), Time.deltaTime * 3f);
    }

    private void StopGrounded()
    {
        grounded = false;
    }

    private void UpdateCameraFOV()
    {
        Camera camera = playerCam.GetComponent<Camera>();

        if (isDashing)
        {
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, dashFOV, Time.deltaTime * FOVChangeSpeed);
        }
        else
        {
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, normalFOV, Time.deltaTime * FOVChangeSpeed);
        }
    }
}
