using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsManager : MonoBehaviour
{
    public GameObject optionsMenu = null;
    public void Open () => optionsMenu.SetActive(true);
    public void Close () => optionsMenu.SetActive(false);
    public static void ShowTooltip (bool state) => Options.showTooltips = state;
    public void ToggleTooltip () => Options.showTooltips = !Options.showTooltips;
}
