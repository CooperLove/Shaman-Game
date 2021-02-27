using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tab : MonoBehaviour
{
    [SerializeField] private GameObject exclamationMark = null;

    public GameObject ExclamationMark { get => exclamationMark; set => exclamationMark = value; }

    public abstract void OpenAction ();
    public abstract void CloseAction ();

    public virtual void NewItemAction() => exclamationMark.SetActive(true);
    public virtual void NoNewItemAction() => exclamationMark.SetActive(false);
}
