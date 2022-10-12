using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] Toggle fullScreenCheckmark;
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider slider;
    [SerializeField] TMP_Dropdown dropdown;
    [SerializeField] Canvas canvas;
    private bool _isFullScreen;
    private Resolution[] resolutions;
    private List<string> resolutionList = new List<string>();
    private List<string> noFpsList;
    void Start()
    {
        slider.value = getVolume();
        resolutions = Screen.resolutions;
        dropdown.ClearOptions();
        foreach (var resolution in resolutions)
        {
            string res = resolution.width + "x" + resolution.height;
            resolutionList.Add(res);
        }
        noFpsList = resolutionList.Distinct().ToList();
        dropdown.AddOptions(noFpsList);
        fullScreenCheckmark.isOn = Screen.fullScreen;
        var reso = Screen.currentResolution;
        dropdown.value = noFpsList.IndexOf(reso.width + "x" + reso.height);
    }
    public float getVolume()
    {
        float value;
        bool result = audioMixer.GetFloat("Volume", out value);
        if (result)
        {
            return value;
        }
        else
        {
            return 0f;
        }
    }
    public void setVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }
    public void setFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        _isFullScreen = isFullScreen;
    }
    public void setResolution(int resolutionIndex)
    {
        var res = noFpsList[resolutionIndex].Split("x");
        Screen.SetResolution(Convert.ToInt32(res[0]), Convert.ToInt32(res[1]), !_isFullScreen);
        fullScreenCheckmark.isOn = !Screen.fullScreen;
    }
    public void goBackToMainMenu()
    {
        canvas.GetComponent<AudioSource>().Play();
        SceneManager.LoadScene("MainMenu");
    }


}
