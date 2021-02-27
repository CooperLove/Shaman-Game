using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Skills/Crocodile/Chain")]
public class Chain : ChainLike
{
    //private float tickDamage = 0f;
    public float spCostPerTick = 0;
    public float mpCostPerTick = 0;
    private void OnDisable() {
        CurrentLevel = 0;
    }

    [SerializeField] private ThrowChain throwChain;
    public override void OnLearn()
    {
        throwChain = Player.Instance.gameObject.AddComponent<ThrowChain>();
        throwChain.skill = this;
        throwChain.key = key;
        
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
        if (throwChain.pulling)
            return false;

        if (throwChain.reachedTarget && throwChain.thrown && !throwChain.canPull && !throwChain.canDoDamage){
            enterOnCooldown = true;
            useSkill.Stop();
            Destroy(throwChain.chain);
            Destroy(throwChain.aim);
        }
        if (!throwChain.thrown && !throwChain.canPull && !throwChain.canDoDamage){
            enterOnCooldown = false;
            return throwChain.Throw();
        }
        if (throwChain.canPull && !throwChain.canDoDamage){
            Debug.Log("Pull chain");
            Player.Instance.StartCoroutine(throwChain.PullTarget());
            Destroy(throwChain.aim);
            enterOnCooldown = true;
        }
        if (!throwChain.canPull && throwChain.canDoDamage){
            Debug.Log("Damage chain");
            throwChain.doingDamage = true;
            Destroy(throwChain.aim);
            Player.Instance.StartCoroutine(throwChain.Damage());
        }
            
        return true;
    }

    
}
