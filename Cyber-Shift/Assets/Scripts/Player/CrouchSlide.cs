using System.Collections;
using UnityEngine;

public class CrouchSlide : Move
{
    [SerializeField] private float slideSpeed = 8f;
    [SerializeField] private float slideDuration = 0.5f;
    // [SerializeField] private Animator animator;

    private bool isSliding;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (Input.GetKey(KeyCode.W) && !isSliding)
            {
                StartCoroutine(Slide());
            }
            else
            {
                Crouch();
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftControl) && isCrouching)
        {
            StandUp();
        }
    }

    private void Crouch()
    {
        isCrouching = true;
        Debug.Log("Crouch");
    }

    private void StandUp()
    {
        isCrouching = false;
        Debug.Log("Stand Up");
    }

    private IEnumerator Slide()
    {
        if (isSliding) yield break;
        isSliding = true;
        IsSliding = true;
        // animator.SetTrigger("Slide");
        Debug.Log("Slide");

        Vector3 slideDirection = transform.forward * slideSpeed;
        rb.velocity = new Vector3(slideDirection.x, rb.velocity.y, slideDirection.z);

        float elapsedTime = 0f;
        while (elapsedTime < slideDuration)
        {
            rb.velocity = new Vector3(slideDirection.x, rb.velocity.y, slideDirection.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isSliding = false;
        IsSliding = false;

        if (Input.GetKey(KeyCode.LeftControl))
        {
            Crouch();
        }
        else
        {
            StandUp();
        }
    }
}
