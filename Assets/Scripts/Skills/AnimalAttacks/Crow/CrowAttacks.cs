using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowAttacks : AnimalAttacks
{
    
    
    PlayerInfo player;
    // Start is called before the first frame update
    void Start()
    {
        canUseBasicAttack     = true;
        canUseFirstSpell    = true;
        canUseSecondSpell = true;
        canUseThirdSpell   = true;
        player = Player.Instance.playerInfo;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
