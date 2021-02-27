using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Skills/Wolf/WolfBuff")]
public class WolfBuff : Active
{
    [SerializeField] private GameObject buffObject;

    public GameObject BuffObject { get => buffObject; set => buffObject = value; }

    [SerializeField] private float ccReduction;
    public float CcReduction { get => ccReduction; set => ccReduction = value; }

    [SerializeField] private float buffDuration;
    
    public float BuffDuration { get => buffDuration; set => buffDuration = value; }

    public override void OnLearn()
    {
        if (Player.Instance.playerInfo.LearnedSkills.Contains(this))
            return;
            
        CurrentLevel = 1;
        Player.Instance.playerInfo.LearnedSkills.Add(this);
    }

    public override void OnUpgrade()
    {
        throw new System.NotImplementedException();
    }

    public override bool OnUse()
    {
        var pi = Player.Instance.playerInfo;
        
        if (pi.Mana <= MpCost){
            GameAnnouncement.NotEnoughManaAnnouncement();
            return false;
        }

        GameObject buff = null;
        if (!buffObject)
            buff = GameObject.Find("Wolf - Buff");
        
        var g = Resources.Load("Prefabs/Combat/Wolf/Wolf - Buff") as GameObject;

        if (g && !buff && !buffObject){
            var wolfBuff = Instantiate(g, Player.Instance.transform.position, g.transform.rotation);
        }else if (buff && !buffObject) {
            buffObject = buff;
            ResetBuff();
        }else if (buffObject) {
            ResetBuff();
        }

        return true;
    }

    private void OnDisable()
    {
        buffObject = null;
    }

    private void ResetBuff()
    {
        if (!buffObject)
            return;
        
        buffObject.SetActive(false);
        buffObject.SetActive(true);
    }
}
