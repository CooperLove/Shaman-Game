using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Skills/Player/Skill 02 Test")]
public class PlayerSkill_Skill02 : Active
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
        Debug.Log("Used skill 02!");
        return false;
    }
}
