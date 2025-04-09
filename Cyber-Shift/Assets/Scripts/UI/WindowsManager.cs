using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WindowsManager : MonoBehaviour
{
    public static WindowsManager Layout;

    [SerializeField] private GameObject[] windows;
    [SerializeField] private bool IsMenu;

    private bool isEndPanelActive = false;

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
        if (!IsMenu && !isEndPanelActive)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ToggleMenu();
            }
        }
    }

    public void SetEndPanelActive(bool state)
    {
        isEndPanelActive = state;
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
        if (isEndPanelActive) return;

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