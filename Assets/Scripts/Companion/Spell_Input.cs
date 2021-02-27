using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_Input : MonoBehaviour
{
    public UpgradableSkill skill;
    public KeyCode key = 0;
    public bool cond;
    // Start is called before the first frame update
    void Start()
    {
        UseSkill useAbility = GetComponent<UseSkill>();
        skill = useAbility.Skill;
    }

    // Update is called once per frame
    

    public void SetKey (int keyCode){
        KeyCode k = (KeyCode) keyCode;
        key = k;
        //Debug.Log($"Key {key} => {keyCode}");
    }

    public int GetKey(int index){
        //Debug.Log($"Index {index}");
        switch (index)
        {
            case 0:
                return 282; // F1
            case 1:
                return 283; // F2
            case 2:
                return 284; // F3
            case 3:
                return 285; // F4
        }
        return -1;
    }
    public int GetKey(){
        switch (transform.GetSiblingIndex())
        {
            case 0:
                return 282; // F1
            case 1:
                return 283; // F2
            case 2:
                return 284; // F3
            case 3:
                return 285; // F4
        }
        return -1;
    }
}
