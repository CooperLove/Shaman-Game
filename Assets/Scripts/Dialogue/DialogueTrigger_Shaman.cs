using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger_Shaman : DialogueTrigger
{
    public override void OnDialogueEnd()
    {
        SceneClipManager.Instance.play = true;
    }
}
