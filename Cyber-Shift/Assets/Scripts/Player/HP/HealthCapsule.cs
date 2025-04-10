using UnityEngine;

public class HealthCapsule : MonoBehaviour
{
    [Header("Heal Settings")]
    [SerializeField] private int healAmount = 60;
    [SerializeField] private float interactionDistance = 3f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip healSound;

    [Header("Visual Feedback")]
    [SerializeField] private GameObject visualObject;

    private bool isUsed = false;
    private Camera playerCamera;

    private void Awake()
    {
        GameObject cameraObj = GameObject.FindGameObjectWithTag("PlayerCamera");
        if (cameraObj != null) playerCamera = cameraObj.GetComponent<Camera>();

        if (visualObject == null) visualObject = gameObject;
    }

    private void Update()
    {
        if (isUsed || playerCamera == null) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, interactionDistance))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    HealPlayer();
                }
            }
        }
    }

    private void HealPlayer()
    {
        IHealthSystem playerHealth = PlayerHealth.Instance;
        if (playerHealth == null) return;

        playerHealth.Heal(healAmount);
        isUsed = true;

        if (audioSource != null && healSound != null)
        {
            audioSource.PlayOneShot(healSound);
        }

        if (visualObject != null)
        {
            visualObject.SetActive(false);
        }

        Collider collider = GetComponent<Collider>();
        if (collider != null) collider.enabled = false;

        float destroyDelay = 0f;

        if (healSound != null) destroyDelay = healSound.length;


        Destroy(gameObject, destroyDelay);
    }
}