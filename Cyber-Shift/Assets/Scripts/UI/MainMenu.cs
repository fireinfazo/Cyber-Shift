using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private Slider loadingSlider;
    [SerializeField] private float minLoadTime = 1.5f;
    [SerializeField] private float fakeLoadDelay = 0.5f;

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
        if (loadingPanel != null)
        {
            loadingPanel.SetActive(true);
        }

        yield return new WaitForSeconds(fakeLoadDelay);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;

        float timer = 0f;

        while (!operation.isDone)
        {
            timer += Time.deltaTime;

            float progress = Mathf.Clamp01(timer / minLoadTime * 0.9f);
            float realProgress = Mathf.Clamp01(operation.progress / 0.9f);

            float displayProgress = Mathf.Min(progress, realProgress);

            if (loadingSlider != null)
            {
                loadingSlider.value = displayProgress;
            }

            if (timer >= minLoadTime && operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}