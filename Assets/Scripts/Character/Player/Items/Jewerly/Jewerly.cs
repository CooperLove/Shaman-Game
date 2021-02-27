using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Jewerly : Armature
{

    public override string GetDescriptionText()
    {
        return "";
    }

    public override Type MyHandler() {
        return typeof(JewerlyHandler);
    }

    public override Type MyType (){
        return typeof(Jewerly);
    }

    public override void NewItemIndicator (bool active){
        InventoryTabManager.Instance.jewerlyTab.ExclamationMark.SetActive(active);
    }
}
