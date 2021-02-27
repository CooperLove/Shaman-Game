using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Active : SkillWithCost, ISerializationCallbackReceiver
{
    [SerializeField] private float gainOfEnergyPerUse;
    [SerializeField] private List<KeyCode> preferredKeys;   // Lista de Botões para se usar a skill

#region Properties
    public List<KeyCode> PreferredKeys { get => preferredKeys; set => preferredKeys = value; }
    public float GainOfEnergyPerUse { get => gainOfEnergyPerUse; set => this.gainOfEnergyPerUse = value; }
#endregion

    public abstract bool OnUse ();

    public void OnBeforeSerialize()
    {
        //Debug.Log("OnBeforeSerialize");
    }

    public void OnAfterDeserialize()
    {
        //Debug.Log("OnAfterDeserialize");
    }
}

[System.Serializable]
public class SerializableActive {
    public int currentLevel;
    public int levelToLearn;
    public int cooldown;
    // public int hpCost;
    // public int mpCost;
    // public int SpCost;
}
