using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuManager : MonoBehaviour
{
    [SerializeField] private Button returnButton;

    [Header("Volume Settings")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private float defaultVolume = 1.0f;

    [Header("Graphics Settings")]
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private Toggle fullScreenToggle;
    public TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;

    private int qualityLevel;
    private bool isFullscreen;

    private void Start()
    {
        SetVolume(AudioListener.volume);
        volumeSlider.value = AudioListener.volume;

        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<String> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    // Audio
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeTextValue.text = "Volume: " + (volume * 100).ToString("00") + "%";
    }

    public void VolumeApply()
    {
        // save volume valume to save data
    }

    // Graphics
    public void SetFullScreen(bool _isFullSsreen)
    {
        isFullscreen = _isFullSsreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution _resolution = resolutions[resolutionIndex];
        Screen.SetResolution(_resolution.width, _resolution.height, Screen.fullScreen);
    }

    public void SetQuality(int qualityIndex)
    {
        qualityLevel = qualityIndex;
    }

    public void GraphicsApply()
    {
        // save quality level to save data
        QualitySettings.SetQualityLevel(qualityLevel);

        // save fullscreen bool to save data
        Screen.fullScreen = isFullscreen;
    }

    public void ApplySettings()
    {
        VolumeApply();
        GraphicsApply();
    }

    public void ReturnToMain()
    {
        enabled = false;
    }

    public void ResetToDefault()
    {
        // Audio
        AudioListener.volume = defaultVolume;
        volumeSlider.value = defaultVolume;
        volumeTextValue.text = "Volume: " + defaultVolume.ToString() + "%";

        // Graphics
        qualityDropdown.value = 1;
        QualitySettings.SetQualityLevel(1);

        fullScreenToggle.isOn = false;
        Screen.fullScreen = false;

        Resolution currentResolution = Screen.currentResolution;
        Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);
        resolutionDropdown.value = resolutions.Length;
        GraphicsApply();
    }
}