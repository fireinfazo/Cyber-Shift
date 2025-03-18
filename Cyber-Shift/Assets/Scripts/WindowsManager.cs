using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowsManager : MonoBehaviour
{
    public static WindowsManager Layout;        // сінглтон для глобльного доступу в проекті

    [SerializeField] private GameObject[] windows;     // це масив, в який ми закинемо всі панелі сцени
    [SerializeField] private GameObject sliderObj;     // це масив, в який ми закинемо всі панелі сцени
    // ссылка на объект слайдер

    private void Awake()
    {
        Layout = this;
        foreach (GameObject window in windows)
        {
            window.SetActive(false);
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            OpenLayout("MainMenu");
            sliderObj.SetActive(false);
            Time.timeScale = 0f;
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            foreach (GameObject window in windows)
            {
                window.SetActive(false);
            }
            sliderObj.SetActive(true);
            Time.timeScale = 1f;
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
}
