using System.Collections;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 6f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float backWalkSpeed = 5f;
    [SerializeField] private float maxSpeed = 11f;
    [SerializeField] private float jumpForce = 5f;
    private float crouchSpeed;

    [SerializeField] private GameObject player;
    public Transform playerCam;
    public Transform orientation;
    [SerializeField] private float mouseSensitivity = 1000f;
    private float xRotation;

    private bool isRun;
    private static bool isMovementBlocked;
    private bool isGrounded;

    [SerializeField] private Rigidbody rb;

    private bool isSliding;
    private bool isCrouching;
    private float baseWalkSpeed;

    private void Start()
    {
        baseWalkSpeed = walkSpeed;
        crouchSpeed = walkSpeed * 0.5f;
    }

    private void Update()
    {
        if (isMovementBlocked) return;

        SlideAndJump();
        HandleMouseLook();
        HandleMovement();
    }


    private void HandleMovement()
    {
        bool isMoving = false;
        Vector3 moveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift) && !isSliding)
        {
            isRun = true;
            moveDirection = transform.forward * runSpeed;
            isMoving = true;
        }
        else if (Input.GetKey(KeyCode.W) && !isSliding)
        {
            isRun = false;
            moveDirection = transform.forward * walkSpeed;
            isMoving = true;
        }

        if (Input.GetKey(KeyCode.S) && !isRun)
        {
            moveDirection += -transform.forward * backWalkSpeed;
            isMoving = true;
        }

        if (Input.GetKey(KeyCode.D) && !isRun)
        {
            moveDirection += transform.right * backWalkSpeed;
            isMoving = true;
        }

        if (Input.GetKey(KeyCode.A) && !isRun)
        {
            moveDirection += -transform.right * backWalkSpeed;
            isMoving = true;
        }

        MovePlayer(moveDirection);
    }

    private void MovePlayer(Vector3 moveDirection)
    {
        Vector3 newVelocity = rb.velocity;
        newVelocity.x = moveDirection.x;
        newVelocity.z = moveDirection.z;

        if (!isSliding && newVelocity.magnitude > maxSpeed)
        {
            newVelocity = newVelocity.normalized * maxSpeed;
        }

        rb.velocity = new Vector3(newVelocity.x, rb.velocity.y, newVelocity.z);
    }

    private float desiredX;

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.fixedDeltaTime * 1f;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.fixedDeltaTime * 1f;

        //Find current look rotation
        Vector3 rot = playerCam.transform.localRotation.eulerAngles;
        desiredX = rot.y + mouseX;

        //Rotate, and also make sure we dont over- or under-rotate.
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Perform the rotations
        playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, 0);
        orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);

        if (Input.GetMouseButtonDown(0))
            Cursor.lockState = CursorLockMode.Locked;
    }



    private void SlideAndJump()
    {
        if (Input.GetKey(KeyCode.LeftControl) && !isSliding)
        {
            isCrouching = true;
            walkSpeed = crouchSpeed;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl) && isCrouching && !isSliding)
        {
            isCrouching = false;
            walkSpeed = baseWalkSpeed;
        }

        isSliding = isCrouching && Input.GetKey(KeyCode.LeftShift);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }

    private void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
}
