using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Added for scene management

public class WindowsManager : MonoBehaviour
{
    public static WindowsManager Layout;        // сінглтон для глобльного доступу в проекті

    [SerializeField] private GameObject[] windows;     // масив всіх панелей сцени

    private void Awake()
    {
        Layout = this;
        foreach (GameObject window in windows)
        {
            window.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    private void ToggleMenu()
    {
        bool anyWindowActive = false;

        foreach (GameObject window in windows)
        {
            if (window.activeSelf)
            {
                anyWindowActive = true;
                break;
            }
        }

        if (anyWindowActive)
        {
            CloseAllWindows();
            Time.timeScale = 1f;
        }
        else
        {
            OpenLayout("MainMenu");
            Time.timeScale = 0f;
        }
    }

    public void OpenLayout(string name)
    {
        foreach (GameObject window in windows)
        {
            if (window.name == name)             // якщо знайдена потрібна панель
            {
                window.SetActive(true);         // то активуй
            }
            else
            {
                window.SetActive(false);        // а всі інші ми вимикаємо
            }
        }
    }

    private void CloseAllWindows()
    {
        foreach (GameObject window in windows)
        {
            window.SetActive(false);
        }
    }

    // New method for exiting to main menu (scene 0)
    public void ExitToMainMenu()
    {
        Time.timeScale = 1f; // Ensure time is running normally
        SceneManager.LoadScene(0); // Load scene with index 0 (main menu)
    }
}