using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Skills/Passive Crocodile")]
public class PassiveCrocodileSO : Passive
{
    public float heal = 0;
    public float damageTaken = 0;

    [SerializeField] private float passiveUpgradeValue = 0.3f;
    public override void OnLearn()
    {
        Player player = Player.Instance;
        player.playerInfo.Passive = this;
        player.TryGetComponent(out PassiveCrocodile passive);
        if (passive == null){
            player.gameObject.AddComponent(typeof(PassiveCrocodile));
        }
        //Ativa a barra que indica a quantidade de energia da passiva
        //Player.Instance.GetComponent<CrocodileAttacks>().passiveBar.SetActive(true);
        if (Player.Instance.playerInfo.LearnedSkills.Contains(this))
            return;
        Player.Instance.playerInfo.LearnedSkills.Add(this);
    }

    public override void OnUpgrade()
    {
        heal += passiveUpgradeValue;
        damageTaken += passiveUpgradeValue;
    }

    public override void OnUse(float value, bool isPercentage = false)
    {
        throw new System.NotImplementedException();
    }

    public override void OnUse()
    {
        Player player = Player.Instance;
        PlayerInfo pi = Player.Instance.playerInfo;

        float dmg = pi.Max_HP * (damageTaken/ 100f);
        if (pi.Health - dmg > 0 )
            player.TakeDamage((int)(dmg));
        
    }

    public void Use (float value){
        Player player = Player.Instance;
        PlayerInfo pi = Player.Instance.playerInfo;
        
        // PassiveCrocodile passive = player.GetComponent<PassiveCrocodile>();
        // passive.Use(value);
        
    }
}
