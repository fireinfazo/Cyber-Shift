using UnityEngine;

public class TurretDisableButton : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private StaticTurret[] turrets;
    private Camera playerCamera;

    [Header("Feedback")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip buttonPressSound;
    [SerializeField] private Light[] lights; // Подключаем Point Light сюда
    [SerializeField] private Color activeColor = Color.green;
    [SerializeField] private Color defaultColor = Color.red;

    private bool isPressed = false;

    private void Awake()
    {
        // Находим камеру по тегу
        if (playerCamera == null)
            playerCamera = GameObject.FindGameObjectWithTag("PlayerCamera")?.GetComponent<Camera>();

        // Инициализация света (если массив не заполнен вручную - попробуем найти автоматически)
        if (lights == null || lights.Length == 0)
        {
            lights = GetComponentsInChildren<Light>(true); // Ищем Light в дочерних объектах
            Debug.LogWarning("Lights array was empty, searching in children... Found: " + lights.Length);
        }

        SetLightsColor(defaultColor); // Устанавливаем начальный цвет
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isPressed)
        {
            TryPressButton();
        }
    }

    private void TryPressButton()
    {
        if (playerCamera == null || isPressed) return;

        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            if (hit.collider.gameObject == gameObject)
            {
                PressButton();
            }
        }
    }

    private void PressButton()
    {
        isPressed = true;

        // Звук
        if (audioSource != null && buttonPressSound != null)
        {
            audioSource.PlayOneShot(buttonPressSound);
        }

        // Меняем цвет света
        SetLightsColor(activeColor);

        // Отключаем турели
        foreach (StaticTurret turret in turrets)
        {
            if (turret != null) turret.DisableTurret();
        }
    }

    // Метод для изменения цвета света (работает с Point Light, Spot Light, Directional Light)
    private void SetLightsColor(Color color)
    {
        if (lights == null || lights.Length == 0)
        {
            Debug.LogError("No lights assigned to the button!");
            return;
        }

        foreach (Light light in lights)
        {
            if (light != null)
            {
                light.color = color;
                Debug.Log($"Changed light {light.name} to {color}");
            }
        }
    }
}