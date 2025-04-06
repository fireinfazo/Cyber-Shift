using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Main Settings")]
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private Transform button1;
    [SerializeField] private Transform button2;
    [SerializeField] private float openDuration = 10f;
    [SerializeField] private bool isLocked = false;

    [Header("Audio")]
    [SerializeField] private AudioClip buttonPressSound;
    [SerializeField] private AudioClip doorOpenSound;
    [SerializeField] private AudioClip lockedSound;
    [SerializeField] private AudioSource audioSource;

    [Header("Visual Feedback")]
    [SerializeField] private Light button1Light;
    [SerializeField] private Light button2Light;

    private bool isReady = true;
    private static Camera playerCamera;
    private Transform currentHoveredButton;

    private void Awake()
    {
        if (playerCamera == null)
            playerCamera = GameObject.FindGameObjectWithTag("PlayerCamera")?.GetComponent<Camera>();

        if (button1Light == null || button2Light == null)
        {
            Light[] lights = GetComponentsInChildren<Light>(true);
            if (lights.Length >= 2)
            {
                button1Light = lights[0];
                button2Light = lights[1];
            }
        }

        UpdateButtonLights();
    }

    private void UpdateButtonLights()
    {
        if (button1Light != null)
            button1Light.color = isLocked ? Color.red : (isReady ? Color.yellow : Color.green);
        if (button2Light != null)
            button2Light.color = isLocked ? Color.red : (isReady ? Color.yellow : Color.green);
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

        if (isLocked)
        {
            PlaySound(lockedSound);
            Invoke(nameof(ResetReadyState), 1f);
        }
        else
        {
            TriggerDoorOpen();
            Invoke(nameof(ResetDoor), openDuration);
            UpdateButtonLights();
        }
    }

    private void ResetReadyState()
    {
        isReady = true;
    }

    private void TriggerDoorOpen()
    {
        doorAnimator.SetTrigger("Open");
        PlaySound(doorOpenSound);
    }

    private void ResetDoor()
    {
        isReady = true;
        UpdateButtonLights();
    }

    private void SetButtonLights(Color color)
    {
        if (button1Light != null) button1Light.color = color;
        if (button2Light != null) button2Light.color = color;
    }

    public void UnlockDoor()
    {
        isLocked = false;
        UpdateButtonLights();
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