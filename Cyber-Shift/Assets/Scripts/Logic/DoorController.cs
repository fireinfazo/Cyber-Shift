using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Main Settings")]
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private Transform button1;
    [SerializeField] private Transform button2;
    [SerializeField] private float openDuration = 10f;

    [Header("Audio")]
    [SerializeField] private AudioClip buttonPressSound;
    [SerializeField] private AudioClip doorOpenSound;

    [SerializeField] private AudioSource audioSource;
    private bool isReady = true;
    private static Camera playerCamera;
    private Transform currentHoveredButton;

    private void Awake()
    {
        //audioSource = gameObject.AddComponent<AudioSource>();
        //audioSource.spatialBlend = 1f;

        if (playerCamera == null)
            playerCamera = GameObject.FindGameObjectWithTag("PlayerCamera")?.GetComponent<Camera>();
    }

    private void Update()
    {
        CheckButtonHover();

        if (Input.GetKeyDown(KeyCode.E) && currentHoveredButton != null && isReady)
        {
            PressButton();
        }
    }

    private void CheckButtonHover()
    {
        if (playerCamera == null || !isReady) return;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        currentHoveredButton = Physics.Raycast(ray, out RaycastHit hit, 3f) && (hit.transform == button1 || hit.transform == button2) ? hit.transform : null;
    }

    private void PressButton()
    {
        isReady = false;
        PlaySound(buttonPressSound);
        TriggerDoorOpen();
        Invoke(nameof(ResetDoor), openDuration);
    }

    private void TriggerDoorOpen()
    {
        doorAnimator.SetTrigger("Open");
        PlaySound(doorOpenSound);
    }

    private void ResetDoor()
    {
        isReady = true;
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
            audioSource.PlayOneShot(clip);
    }

    private void OnValidate()
    {
        if (doorAnimator == null)
            doorAnimator = GetComponent<Animator>();
    }
}