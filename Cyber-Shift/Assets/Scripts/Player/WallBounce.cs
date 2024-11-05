using UnityEngine;
using System.Collections;

public class WallBounce : MonoBehaviour
{
    public float bounceForce = 10f;
    private bool isTouchingWall = false;
    private Vector3 wallNormal;
    private bool canBounce = true;

    private void Update()
    {
        if (isTouchingWall && Input.GetKeyDown(KeyCode.Space) && canBounce)
        {
            Bounce();
            Debug.Log(canBounce);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            isTouchingWall = true;
            wallNormal = collision.contacts[0].normal;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            isTouchingWall = false;
        }
    }

    private void Bounce()
    {
        StartCoroutine(ReloadBounce());
        Vector3 bounceDirection = wallNormal * -1;
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = new Vector3(bounceDirection.x * bounceForce, bounceForce, bounceDirection.z * bounceForce);
            canBounce = false;
            Debug.Log("Bounce");
        }
    }

    private IEnumerator ReloadBounce()
    {
        yield return new WaitForSeconds(3f);
        canBounce = true; 
    }
}
