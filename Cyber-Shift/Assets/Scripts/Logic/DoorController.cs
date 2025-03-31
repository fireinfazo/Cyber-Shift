using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip denySound;
    [SerializeField] private bool requiresCondition = false;
    [SerializeField] private InteractionCondition condition;
    [SerializeField] private float interactionRange = 3f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private GameObject interactableObject1;
    [SerializeField] private GameObject interactableObject2;

    private bool isLookingAtObject = false;
    private Transform playerCamera;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Transform cameraTransform = player.transform.Find("MainCamera");
            if (cameraTransform != null)
            {
                playerCamera = cameraTransform;
            }
            else
            {
                playerCamera = Camera.main?.transform;
            }
        }
        else
        {
            playerCamera = Camera.main?.transform;
        }

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (playerCamera == null) return;

        CheckForInteractable();

        if (isLookingAtObject && Input.GetKeyDown(KeyCode.E))
        {
            TryOpenDoor();
        }
    }

    private void CheckForInteractable()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * interactionRange, Color.green);

        if (Physics.Raycast(ray, out hit, interactionRange, interactableLayer))
        {
            if (hit.collider.gameObject == interactableObject1 || hit.collider.gameObject == interactableObject2)
            {
                isLookingAtObject = true;
                return;
            }
        }

        isLookingAtObject = false;
    }

    private void TryOpenDoor()
    {
        if (!requiresCondition || (condition != null && condition.IsMet()))
        {
            doorAnimator.SetTrigger("Open");
            if (openSound != null)
                audioSource.PlayOneShot(openSound);
        }
        else
        {
            if (denySound != null)
                audioSource.PlayOneShot(denySound);
        }
    }
}

public class InteractionCondition : MonoBehaviour
{
    public virtual bool IsMet()
    {
        return false;
    }
}
