using UnityEngine;

public class DoorUnlocker : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private DoorController doorToUnlock;
    [SerializeField] private float interactionDistance = 3f;

    [Header("Audio")]
    [SerializeField] private AudioClip leverSound;
    [SerializeField] private AudioSource audioSource;

    [Header("Visual Feedback")]
    [SerializeField] private Light leverLight;

    private bool isActivated = false;
    private static Camera playerCamera;

    private void Awake()
    {
        if (playerCamera == null)
            playerCamera = GameObject.FindGameObjectWithTag("PlayerCamera")?.GetComponent<Camera>();

        if (leverLight != null)
            leverLight.color = Color.red;
    }

    private void Update()
    {
        if (isActivated) return;

        if (Input.GetKeyDown(KeyCode.E) && IsLookingAtLever())
        {
            ActivateLever();
        }
    }

    private bool IsLookingAtLever()
    {
        if (playerCamera == null) return false;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        return Physics.Raycast(ray, out RaycastHit hit, interactionDistance) && hit.transform == transform;
    }

    private void ActivateLever()
    {
        isActivated = true;
        PlaySound(leverSound);

        if (doorToUnlock != null)
            doorToUnlock.UnlockDoor();
        if (leverLight != null)
            leverLight.color = Color.green;
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
            audioSource.PlayOneShot(clip);
    }
}