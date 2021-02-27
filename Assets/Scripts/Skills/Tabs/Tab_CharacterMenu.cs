using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tab_CharacterMenu : Tab
{
    public override void OpenAction()
    {
        Debug.Log("Open Character Menu");
        Inventory.Instance.HighlightTab(GetComponent<Button>());
        CompanionUI.Instance.OpenCharacterMenuUI();
        CompanionUI.Instance.OpenCharacterStatistics();
        AddPoints.Instance.UpdateAllTexts();
        CharacterMenuUI.Instance.ChangeUI_Images();
    }
    public override void CloseAction()
    {
        //CompanionUI.Instance.CloseCharacterStatistics();
        CompanionUI.Instance.CloseCharacterMenuUI();
    }
}
