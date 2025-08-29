using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider progressBar;
    [SerializeField] private GameObject continueText;

    private bool isReadyToContinue = false;

    public void StartGame(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        loadingScreen.SetActive(true);
        continueText.SetActive(false);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            if (progressBar != null)
                progressBar.value = progress;

            if (operation.progress >= 0.9f && !isReadyToContinue)
            {

                isReadyToContinue = true;
                continueText.SetActive(true);
            }

            if (isReadyToContinue && Input.anyKeyDown)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
