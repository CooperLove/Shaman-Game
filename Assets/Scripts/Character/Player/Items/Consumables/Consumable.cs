using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Consumable : Collectable
{
    public abstract void Use ();
    public override Type MyHandler() {
        return typeof(ConsumableHandler);
    }

    public override Type MyType (){
        return typeof(Consumable);
    }

    public override void NewItemIndicator (bool active){
        InventoryTabManager.Instance.consumableTab.ExclamationMark.SetActive(active);
    }
}
