using System;
using System.Collections;
using System.Collections.Generic;
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
    //    public Animator animator;

    [SerializeField] private float mouseSensitivity = 1000f;
    private float yRotation = 0f;

    //    private static bool isAnimation = true;
    //    private bool isAnimationRun = true;

    private bool isRun;

    private static bool isMovementBlocked;

    private bool isGrounded;

    [SerializeField] protected Rigidbody rb;

    protected bool IsSliding;
    protected bool IsCrouch;

    private float baseWalkSpeed;


    private void Start()
    {
        baseWalkSpeed = walkSpeed;
        crouchSpeed = walkSpeed*0.5f;
    }

    private void Update()
    {
        if (isMovementBlocked)
        {
            StopMovementAnimations();
            return;
        }

        HandleMovement();
        HandleMouseLook();
    }

    protected void HandleMovement()
    {
        bool isMoving = false;
        Vector3 moveDirection = Vector3.zero;
        IsSliding = SlideCrouch.IsSliding();

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift) && !isMovementBlocked && !IsSliding)
        {
            isRun = true;
            moveDirection = transform.forward * runSpeed;
            isMoving = true;
            //            RunAnimation();
        }

        else if (Input.GetKey(KeyCode.W) && !isMovementBlocked && !IsSliding)
        {
            isRun = false;
            moveDirection = transform.forward * walkSpeed;
            isMoving = true;
            //            WalkAnimation();
        }

        if (Input.GetKey(KeyCode.LeftControl) && !isMovementBlocked && !IsSliding)
        {
            IsCrouch = true;
            Debug.Log("Crouch");
            walkSpeed = crouchSpeed;
        }

        if (Input.GetKeyUp(KeyCode.LeftControl) && IsCrouch && !IsSliding)
        {
            IsCrouch = false;
            Debug.Log("Stand Up");
            walkSpeed = baseWalkSpeed;
        }

        if (Input.GetKey(KeyCode.S) && !isRun && !isMovementBlocked)
        {
            moveDirection += -transform.forward * backWalkSpeed;
            //            WalkAnimation();
            isMoving = true;
        }

        if (Input.GetKey(KeyCode.D) && !isRun && !isMovementBlocked)
        {
            moveDirection += transform.right * backWalkSpeed;
            //            WalkAnimation();
            isMoving = true;
        }

        if (Input.GetKey(KeyCode.A) && !isRun && !isMovementBlocked)
        {
            moveDirection += -transform.right * backWalkSpeed;
            //            WalkAnimation();
            isMoving = true;
        }

        MovePlayer(moveDirection);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isMovementBlocked)
        {
            Jump();
        }

        if (!isMoving)
        {
            //            StopMovementAnimations();
        }

        //        MoveSound(isMoving);
        if (IsSliding)
        {
            walkSpeed = baseWalkSpeed;
        }
    }

    private void MovePlayer(Vector3 moveDirection)
    {
        Vector3 newVelocity = rb.velocity;

        newVelocity.x = moveDirection.x;
        newVelocity.z = moveDirection.z;

        if (!IsSliding && newVelocity.magnitude > maxSpeed)
        {
            newVelocity = newVelocity.normalized * maxSpeed;
        }

        rb.velocity = new Vector3(newVelocity.x, rb.velocity.y, newVelocity.z);
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        yRotation += mouseX;
        transform.localRotation = Quaternion.Euler(0f, yRotation, 0f);
        isRun = false;
    }

    /*
        #region Animation
        private void WalkAnimation()
        {
            if (isAnimation)
            {
                animator.SetBool("WalkTrigger", true);
                isAnimationRun = true;
            }
        }

        private void RunAnimation()
        {
            if (isAnimationRun)
            {
                animator.SetBool("Run", true);
                isAnimation = true;
            }
        }

        public void WalkAnimationFinish()
        {
            isAnimation = true;
        }

        public void WalkAnimationStart()
        {
            animator.SetBool("WalkTrigger", false);
            animator.SetBool("Run", false);
            isAnimation = false;
        }

        public void RunAnimationFinish()
        {
            isAnimationRun = true;
        }

        public void RunAnimationStart()
        {
            animator.SetBool("Run", false);
            animator.SetBool("WalkTrigger", false);
            isAnimationRun = false;
        }

        public static void TrueAnimation()
        {
            isAnimation = true;
        }
        #endregion

        */

    #region BlockMovement
    public static void BlockMovement(float seconds)
    {
        Move instance = FindObjectOfType<Move>();
        instance.StartCoroutine(instance.WaitBlockMovement(seconds));
    }

    private IEnumerator WaitBlockMovement(float seconds)
    {
        isMovementBlocked = true;
        yield return new WaitForSeconds(seconds);
        isMovementBlocked = false;
    }

    public static void BlockMovementBool(bool block)
    {
        isMovementBlocked = block;
    }

    #endregion

    #region Jump
    private void Jump()
    {
        //        animator.SetTrigger("Jump");
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
    #endregion

    private void StopMovementAnimations()
    {
        //animator.SetBool("WalkTrigger", false);
        //animator.SetBool("Run", false);
    }

    /*
    private void MoveSound(bool isMoving)
    {
        if (isMoving && isRun)
        {
            if (!audios[0].isPlaying)
            {
                audios[1].Stop();
                audios[0].loop = true;
                audios[0].Play();
            }
        }
        else if (isMoving && !isRun)
        {
            if (!audios[1].isPlaying)
            {
                audios[0].Stop();  
                audios[1].loop = true;
                audios[1].Play();
            }
        }
        else
        {
            if (audios[0].isPlaying)
            {
                audios[0].Stop();
            }
            if (audios[1].isPlaying)
            {
                audios[1].Stop();
            }
        }
    }
    */
}