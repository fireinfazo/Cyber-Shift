using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WindowsManager : MonoBehaviour
{
    public static WindowsManager Layout;        // сінглтон для глобльного доступу в проекті

    [SerializeField] private GameObject[] windows;     // масив всіх панелей сцени

    [SerializeField] private bool IsMenu;

    private void Awake()
    {
        Layout = this;
        foreach (GameObject window in windows)
        {
            window.SetActive(false);
        }
    }

    private void Start()
    {
        if (IsMenu)
        {
            ToggleMenu();
        }
    }

    void Update()
    {
        if (!IsMenu)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ToggleMenu();
            }
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
            if (!IsMenu)
                Time.timeScale = 1f;
        }
        else
        {
            OpenLayout("MainMenu");
            if (!IsMenu)
                Time.timeScale = 0f;
        }
    }

    public void OpenLayout(string name)
    {
        foreach (GameObject window in windows)
        {
            if (window.name == name)
            {
                window.SetActive(true);
            }
            else
            {
                window.SetActive(false);
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

    public void ExitToMainMenu()
    {
        if (!IsMenu)
            Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}