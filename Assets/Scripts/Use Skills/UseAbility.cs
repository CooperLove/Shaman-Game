using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UseAbility : MonoBehaviour
{
    [SerializeField] public Image cooldownBG;
    public bool canUseSpell, updateBG;

    public abstract void Use();
    public abstract void Stop();
    public abstract bool ToggleSkill();
    public abstract void HideUI ();
}
