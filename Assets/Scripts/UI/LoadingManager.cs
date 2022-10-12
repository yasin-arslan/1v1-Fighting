using System.Collections;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class LoadingManager : MonoBehaviour
{
    /// <summary>
    ///Yükleniyor ekranı efekti için basit bir script.
    ///</summary>
    [SerializeField] Canvas canvas;
    [SerializeField] GameObject loadingScreen;
    [SerializeField] Slider slider;
    [SerializeField] TMP_Text percentage;
    public void loadLevel(string levelName)
    {
        StartCoroutine(loadLevelAsync(levelName));
    }
    IEnumerator loadLevelAsync(string sceneName)
    {
        playClickSound();
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        loadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;
            percentage.text = String.Format("%{0}", (progress * 100f).ToString());
            yield return null;
        }
    }
    public void playClickSound()
    {
        canvas.GetComponent<AudioSource>().Play();
    }
    public void exitGame()
    {
        Application.Quit();
    }
}
