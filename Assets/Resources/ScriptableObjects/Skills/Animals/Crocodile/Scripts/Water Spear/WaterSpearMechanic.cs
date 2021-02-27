using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSpearMechanic : SkillMechanic
{
    // Start is called before the first frame update
    private void Start()
    {
        if (!Player.Instance.IsFacingRight)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    public override void ApplyDamage()
    {
        throw new System.NotImplementedException();
    }
}
