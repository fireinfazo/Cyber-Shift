using System.Collections;
using UnityEngine;

public class SlideCrouch : MonoBehaviour
{
    [SerializeField] private float slideSpeed = 8f;
    [SerializeField] private float slideDuration = 0.5f;
    // [SerializeField] private Animator animator;

    static private bool isSliding;

    private Rigidbody rb;



    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && Input.GetKey(KeyCode.W) && !isSliding)
        {
            StartCoroutine(Slide());
        }
    }

    private IEnumerator Slide()
    {
        if (isSliding) yield break;
        isSliding = true;
        // animator.SetTrigger("Slide");

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
    }

    static public bool IsSliding()
    {
        return isSliding;
    }
}