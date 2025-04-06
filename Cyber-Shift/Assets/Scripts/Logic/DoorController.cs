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
            else
            {
                Debug.LogWarning("Не найдены оба света. Добавьте два Point Light в инспекторе.");
            }
        }

        if (button1Light != null) button1Light.color = Color.red;
        if (button2Light != null) button2Light.color = Color.red;
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

        if (currentHoveredButton == button1 && button1Light != null)
            button1Light.color = Color.green;
        else if (currentHoveredButton == button2 && button2Light != null)
            button2Light.color = Color.green;
    }

    private void TriggerDoorOpen()
    {
        doorAnimator.SetTrigger("Open");
        PlaySound(doorOpenSound);
    }

    private void ResetDoor()
    {
        isReady = true;

        if (button1Light != null) button1Light.color = Color.red;
        if (button2Light != null) button2Light.color = Color.red;
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