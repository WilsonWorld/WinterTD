using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class OptionsScreen : MonoBehaviour
{
    public Toggle FullscreenToggle, VsyncToggle;
    public List<ResItem> Resolutions = new List<ResItem>();
    public TMP_Text ResolutionLabel;
    public AudioMixer TheMixer;
    public TMP_Text MasterLabel, MusicLabel, SFXLabel;
    public Slider MasterSlider, MusicSlider, SFXSlider;

    private int selectedResolution;

    private void Start()
    {
        FullscreenToggle.isOn = Screen.fullScreen;

        if (QualitySettings.vSyncCount == 0) 
            VsyncToggle.isOn = false;
        else 
            VsyncToggle.isOn = true;

        // Check if the user is using a pre-defined resolution, otherwise add the native display to the list and set the resolution to that
        bool foundRes = false;
        for (int i = 0; i < Resolutions.Count; i++) {

            if (Screen.width == Resolutions[i].Horizontal && Screen.height == Resolutions[i].Vertical) {
                foundRes = true;
                selectedResolution = i;
                UpdateResLabel();
            }
        }

        if (!foundRes) {
            ResItem newRes = new ResItem();
            newRes.Horizontal = Screen.width;
            newRes.Vertical = Screen.height;

            Resolutions.Add(newRes);
            selectedResolution = Resolutions.Count - 1;

            UpdateResLabel();
        }

        // Set the sliders to the previously saved values.
        float vol = 0.0f;
        TheMixer.GetFloat("MasterVol", out vol);
        MasterSlider.value = vol;
        TheMixer.GetFloat("MusicVol", out vol);
        MusicSlider.value = vol;
        TheMixer.GetFloat("SFXVol", out vol);
        SFXSlider.value = vol;

        // Set the labels to display the previously saved values
        MasterLabel.text = Mathf.RoundToInt(MasterSlider.value + 80).ToString();
        MusicLabel.text = Mathf.RoundToInt(MusicSlider.value + 80).ToString();
        SFXLabel.text = Mathf.RoundToInt(SFXSlider.value + 80).ToString();
    }

    public void ResLeft()
    {
        selectedResolution--;

        if (selectedResolution < 0)
            selectedResolution = 0;

        UpdateResLabel();
    }

    public void ResRight()
    {
        selectedResolution++;

        if (selectedResolution > Resolutions.Count - 1)
            selectedResolution = Resolutions.Count - 1;

        UpdateResLabel();
    }

    public void UpdateResLabel()
    {
        ResolutionLabel.text = Resolutions[selectedResolution].Horizontal.ToString() + " x " + Resolutions[selectedResolution].Vertical.ToString();
    }

    public void ApplyGraphics()
    {
        if (VsyncToggle.isOn) 
            QualitySettings.vSyncCount = 1;
        else 
            QualitySettings.vSyncCount = 0;

        Screen.SetResolution(Resolutions[selectedResolution].Horizontal, Resolutions[selectedResolution].Vertical, FullscreenToggle.isOn);
    }

    public void SetMasterVol()
    {
        MasterLabel.text = Mathf.RoundToInt(MasterSlider.value + 80).ToString();
        TheMixer.SetFloat("MasterVol", MasterSlider.value);
        PlayerPrefs.SetFloat("MasterVol", MasterSlider.value);
    }

    public void SetMusicVol()
    {
        MusicLabel.text = Mathf.RoundToInt(MusicSlider.value + 80).ToString();
        TheMixer.SetFloat("MusicVol", MusicSlider.value);
        PlayerPrefs.SetFloat("MusicVol", MusicSlider.value);
    }

    public void SetSFXVol()
    {
        SFXLabel.text = Mathf.RoundToInt(SFXSlider.value + 80).ToString();
        TheMixer.SetFloat("SFXVol", SFXSlider.value);
        PlayerPrefs.SetFloat("SFXVol", SFXSlider.value);
    }
}

[System.Serializable]
public class ResItem
{
    public int Horizontal, Vertical;
}