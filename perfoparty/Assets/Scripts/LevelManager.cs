using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    //[SerializeField] SettingsValuesSO settings;
    [SerializeField] string fullScreenSettingName;
    [SerializeField] GameObject loadingScreen;
    [SerializeField] Image loadingBar;
    [SerializeField] bool loadingScreenActiveOnStart;

    public GameObject GetLoadingScreen => loadingScreen;

    private void Start()
    {
/*        for (int i = 0; i < settings.boolSettings.Length; i++)
        {
            if (settings.boolSettings[i].valueName == fullScreenSettingName)
                settings.boolSettings[i].onSettingsChanges += UpdateFullScreen;
        }*/
        loadingScreen.SetActive(loadingScreenActiveOnStart);
    }

    private IEnumerator LoadLevel(AsyncOperation loadingOperation)
    {
        if (loadingBar != null)
        {
            SetLoadingBarAmount(0);
            while (!loadingOperation.isDone)
            {
                SetLoadingBarAmount(Mathf.Lerp(loadingBar.fillAmount, loadingOperation.progress, Time.deltaTime * 10f));
                yield return null;
            }
        }
    }

    public void LoadNextLevel()
    {
        StartCoroutine(Load());

        IEnumerator Load()
        {
            loadingScreen.SetActive(true);
            yield return new WaitForSeconds(3f);
            AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
            StartCoroutine(LoadLevel(loadingOperation));
        }
    }

    public void LoadCurrentLevel()
    {
        StartCoroutine(Load());

        IEnumerator Load()
        {
            loadingScreen.SetActive(true);
            yield return new WaitForSeconds(3f);
            AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            StartCoroutine(LoadLevel(loadingOperation));
        }
    }

    public void LoadLevelByIndex(int levelIndex)
    {
        StartCoroutine(Load());

        IEnumerator Load()
        {
            loadingScreen.SetActive(true);
            yield return new WaitForSeconds(3f);
            AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(levelIndex);
            StartCoroutine(LoadLevel(loadingOperation));
        }
    }

    public void LoadLevelByString(string levelName)
    {
        StartCoroutine(Load());

        IEnumerator Load()
        {
            loadingScreen.SetActive(true);
            yield return new WaitForSeconds(3f);
            AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(levelName);
            StartCoroutine(LoadLevel(loadingOperation));
        }
    }

    public static void ExitGame() => Application.Quit();

    public void SetLoadingBarAmount(float fillAmount)
    {
        if (!loadingBar) return;
        loadingBar.fillAmount = fillAmount;
    }
}
