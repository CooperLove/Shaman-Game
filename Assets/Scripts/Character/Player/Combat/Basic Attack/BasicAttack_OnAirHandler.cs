using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BasicAttack_OnAirHandler : MonoBehaviour
{
    private Player player;
    private BasicAttack_Handler aaHandler;
    private BasicAttack waitAuto;
    [SerializeField] private bool attacking;
    
    private Coroutine onAirCoroutine;
    private Coroutine nextAutoCoroutine;

    [SerializeField] private float airDropTimer;
    [SerializeField] private float airTimer;
    
    private static BasicAttack_OnAirHandler instance;

    public static BasicAttack_OnAirHandler Instance
    { get => instance; private set => instance = value; }

    private BasicAttack_OnAirHandler()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        player = Player.Instance;
        aaHandler = BasicAttack_Handler.Instance;
    }

    private void Update()
    {
        if (!attacking)
            return;
        
        if (!player.OnGround)
            AirAttackHandler();
        else
        {
            if (airTimer != 0)
                airTimer = 0f;
        }
    }

    public void AutoAttackHandler(BasicAttack basicAttack)
    {
        // Debug.Log($"Air Handler {basicAttack?.name}");
        if (basicAttack == null)
            return;
        
        if (!basicAttack.CanUse)
            return;
        if (player.OnGround) {
           aaHandler.AutoAttackHandler(basicAttack);
           return;
        }
        
        if (!aaHandler.HasStamina(basicAttack.StaminaCost))
            return;
        
        InstantiateBasicAttack(basicAttack);
        aaHandler.NextAutoReset (basicAttack.rightNode ? basicAttack.rightNode.ResetTime : (basicAttack.leftNode ? basicAttack.leftNode.ResetTime : 1.5f));
        aaHandler.WaitAutoAttack(basicAttack.IsSinglePath ? 0 : basicAttack.WaitTime);
        
        // if (onAirCoroutine != null) 
        //     StopCoroutine(onAirCoroutine);
        // onAirCoroutine = StartCoroutine(AirAttackHandler (basicAttack));
        airDropTimer = basicAttack.AirDuration;
        airTimer -= airTimer;
        aaHandler.airAttacks.Add(basicAttack);
        attacking = true;

        // Antes de mudar para o próximo AA
        basicAttack.CanUse = false;
        var animationLength = basicAttack.AnimationLength;
        
        basicAttack = basicAttack.airNode;

        if (basicAttack != null)
            nextAutoCoroutine = StartCoroutine(aaHandler.LetNextAutoGoOff(animationLength, basicAttack));

        aaHandler.PreventPlayerMovement();
        
        aaHandler.timer = 0f;
        
        if (basicAttack) 
            return;
        
        StartCoroutine(aaHandler.ResetCombo (animationLength));
    }
    
    private void InstantiateBasicAttack (BasicAttack basicAttack)
    {
        var trans = transform;
        var position = trans.position;
        var pos = new Vector3(position.x, position.y, basicAttack.transform.position.z);
        var g = Instantiate(basicAttack.gameObject, pos, basicAttack.transform.rotation);
        if(basicAttack.SetPlayerAsParent)
            g.transform.SetParent(player.transform);
    }
    
    
    private IEnumerator AirAttackHandler (BasicAttack basicAttack){
        //Debug.Log($"Using {basicAttack.name} on air");
        aaHandler.airAttacks.Add(basicAttack);
        player.Rb.constraints = RigidbodyConstraints2D.FreezeAll;
        
        yield return new WaitForSeconds(basicAttack.AirDuration);
        //Debug.Log($"Player air duration {basicAttack.AirDuration}");
        player.Rb.velocity += Physics2D.gravity;
        player.Rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    

    private void AirAttackHandler()
    {
        if (player.OnGround)
            return;
        
        if (airTimer < airDropTimer)
        {
            player.Rb.constraints = RigidbodyConstraints2D.FreezeAll;
            airTimer += Time.deltaTime;
            return;
        }
        
        attacking = false;
        airTimer -= airTimer;
        player.Rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
