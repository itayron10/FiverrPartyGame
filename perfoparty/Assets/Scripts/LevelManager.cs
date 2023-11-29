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

    private void UpdateFullScreen(bool fullScreenActive)
    {
        Screen.fullScreen = fullScreenActive;
    }

    private IEnumerator LoadLevel(AsyncOperation loadingOperation)
    {
        loadingScreen.SetActive(true);
        if (loadingBar !=null)
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
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        StartCoroutine(LoadLevel(loadingOperation));
    }

    public void LoadCurrentLevel()
    {
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        StartCoroutine(LoadLevel(loadingOperation));
    }

    public void LoadLevelByIndex(int levelIndex)
    {
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(levelIndex);
        StartCoroutine(LoadLevel(loadingOperation));
    }

    public void LoadLevelByString(string levelName)
    {
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(levelName);
        StartCoroutine(LoadLevel(loadingOperation));
    }

    public static void ExitGame() => Application.Quit();

    public void SetLoadingBarAmount(float fillAmount)
    {
        if (!loadingBar) return;
        loadingBar.fillAmount = fillAmount;
    }
}
