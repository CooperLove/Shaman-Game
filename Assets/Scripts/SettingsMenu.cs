using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public Slider volumeSlider = null;
    public Slider brightnessSlider = null;
    public Slider dpiSlider = null;
    public TMP_Dropdown resolution = null;
    Resolution[] resolutions;
    // Update is called once per frame
    void Start (){
        if (volumeSlider != null) volumeSlider.value = 1.0f;
        if (brightnessSlider != null) brightnessSlider.value = Screen.brightness;
        if (dpiSlider != null) dpiSlider.value = Screen.dpi;
        //resolution.AddOptions(new List<string>{"640x480", "800x600", "1024x768", "1280x960", "1400x1050", "1600x1200", "1920x1440"});
        resolutions = Screen.resolutions;
        List<string> reso = new List<string>();
        // Print the resolutions
        foreach (var res in resolutions)
            reso.Add($"{res.width} x {res.height} {res.refreshRate} hz");
        
        resolution?.ClearOptions();
        resolution?.AddOptions(reso);
        Debug.Log($"{Screen.currentResolution} dpi {Screen.dpi}");
    }
    public void ChangeMainVolume() => AudioListener.volume = volumeSlider.value;

    public void ChangeBrightness () {
        Screen.brightness = brightnessSlider.value;
        Debug.Log($"Brilho {Screen.brightness}");
    }

    public void SetResolution (){
        Debug.Log($"Option {resolution.value} => {resolution.captionText.text}");
        if (!resolutions[resolution.value].Equals(null))
            Screen.SetResolution(resolutions[resolution.value].width, resolutions[resolution.value].height, true);
        //Screen.SetResolution();
    }

    public void FullScreen () => Screen.fullScreen = !Screen.fullScreen;

    public void DPI () {
        QualitySettings.resolutionScalingFixedDPIFactor = dpiSlider.value;
        Debug.Log($"DPI {Screen.dpi}");
    }
}
