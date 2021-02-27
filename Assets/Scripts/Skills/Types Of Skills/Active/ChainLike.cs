using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChainLike : Active
{
    public KeyCode key;
    public UseSkill useSkill;
    
    public bool enterOnCooldown = false;

    public void SetKey (KeyCode k) => key = k;
    public void SetUseSkill (UseSkill u) => useSkill = u;

}
