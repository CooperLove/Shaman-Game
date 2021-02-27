using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tab_Companion : Tab
{
    public override void OpenAction()
    {
        CompanionUI.Instance.OpenCompanionsTab();
    }
    public override void CloseAction()
    {
        CompanionUI.Instance.CloseCompanionWindows();
    }
}
