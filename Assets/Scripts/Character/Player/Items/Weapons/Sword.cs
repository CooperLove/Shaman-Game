using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Sword : Weapon
{
    [SerializeField] Bonus bonus = null;

    

    public void Test (){
        bonus.Apply(this);
        //Player.Instance.playerInfo.AnimalPath = ScriptableObject.CreateInstance<Path_Bear>();
        //GameObject instance = Instantiate(Resources.Load("Asset/Prefabs/Sword01.prefab"), typeof(GameObject)) ;
    }
}
