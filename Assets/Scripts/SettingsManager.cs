using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;


public class SettingsManager : MonoBehaviour
{
    public static SettingsManager instance;
    private static int m_referenceCount = 0;
    Resolution[] resolutions;

    public TMP_Dropdown resolutionUI;
    public AudioMixer mixer;

    void Awake()
    {
        m_referenceCount++;
        if (m_referenceCount > 1)
        {
            DestroyImmediate(this.gameObject);
            return;
        }

        instance = this;
        // Use this line if you need the object to persist across scenes
        DontDestroyOnLoad(this.gameObject);
    }
    void OnDestroy()
    {
        m_referenceCount--;
        if (m_referenceCount == 0)
        {
            instance = null;
        }

    }
    void Start()
    {
        resolutions= Screen.resolutions;
        resolutionUI.ClearOptions();
        int currentREs = 0;
  
        List<string> options = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            if(!options.Contains(option))
                options.Add(option);
            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentREs = i;
            }

        }
        resolutionUI.AddOptions(options);
        resolutionUI.value = currentREs;
        resolutionUI.RefreshShownValue();
    }



    public void CloseSettings()
    {
        GM.Instance.InitialSetup();
        gameObject.SetActive(false);

    }

    public void SetVolume(float volume)
    {
        mixer.SetFloat("Volume", Mathf.Log10(volume) * 20);
    }
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height,Screen.fullScreen);
    }
}
