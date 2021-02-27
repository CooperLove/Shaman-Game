using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Skills/Player/Skill 01 Test")]
public class PlayerSkill_Skill01 : Active
{
    public override void OnLearn()
    {
        throw new System.NotImplementedException();
    }

    public override void OnUpgrade()
    {
        
    }

    public override bool OnUse()
    {
        Debug.Log("Used skill 01!");
        return false;
    }
}
