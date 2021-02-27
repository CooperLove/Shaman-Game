using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuestUI_Input : MonoBehaviour
{

    [SerializeField] private GameObject questButton = null;

    private Animator animator = null;

    private void Start() {
        animator = GetComponent<Animator>();
        questButton = Resources.FindObjectsOfTypeAll<GameObject>().Where(x => x.name.Equals("Quest Button - QB Panel")).ToList()[0];
    }

    public void Open () => animator.Play("Quest Panel Animate Out");
    public void Close () => animator.Play("Quest Panel Animate In");

    public void ActivateQuestButton () => questButton.SetActive(!questButton.activeInHierarchy);
}


