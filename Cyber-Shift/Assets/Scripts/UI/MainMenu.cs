using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private Slider loadingSlider;
    [SerializeField] private float minLoadTime = 1.5f; // Минимальное время загрузки
    [SerializeField] private float fakeLoadDelay = 0.5f; // Задержка перед началом загрузки

    public void playGame()
    {
        StartCoroutine(LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void quitGame()
    {
        Application.Quit();
    }

    private IEnumerator LoadSceneAsync(int sceneIndex)
    {
        // Активируем панель загрузки
        if (loadingPanel != null)
        {
            loadingPanel.SetActive(true);
        }

        // Небольшая задержка перед началом загрузки (для демонстрации панели)
        yield return new WaitForSeconds(fakeLoadDelay);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;

        float timer = 0f;

        while (!operation.isDone)
        {
            timer += Time.deltaTime;

            // Рассчитываем прогресс (искусственный + реальный)
            float progress = Mathf.Clamp01(timer / minLoadTime * 0.9f);
            float realProgress = Mathf.Clamp01(operation.progress / 0.9f);

            // Используем максимальное значение из искусственного и реального прогресса
            float displayProgress = Mathf.Min(progress, realProgress);

            if (loadingSlider != null)
            {
                loadingSlider.value = displayProgress;
            }

            // Когда прошло минимальное время и загрузка действительно завершена
            if (timer >= minLoadTime && operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}