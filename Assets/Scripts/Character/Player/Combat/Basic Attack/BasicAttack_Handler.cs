using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class BasicAttack_Handler : MonoBehaviour
{
    private static BasicAttack_Handler instance;

    public static BasicAttack_Handler Instance
    { get => instance; private set => instance = value; }

    private BasicAttack_Handler()
    {
        if (instance == null)
            instance = this;
    }

    // EVENTOS
    private Action OnHit;
    public Action<Character> OnHitCharacter;

    //public List<BasicAttack_Node> basicAttacks = new List<BasicAttack_Node>();
    [SerializeField] private BasicAttack basicAttack;
    private BasicAttack waitAuto;
    private BasicAttack firstAuto;
    public Vector3 pos;
    
    [Header("Timers")]
    public float timer;


    [Tooltip("Timer to wait to use the right auto node")] 
    private float waitAutoTimer;


    [Tooltip("Timer to reset the autos")] 
    private float resetTimer;
    private float attackMovementTimer = 0f;
    private float attackMovementResetTimer = 0f;
    
    //private bool attacking = false;
    public bool goRight = false;
    private Player player;
    private PlayerInfo playerInfo;
    private Coroutine nextAutoCoroutine;
    private BasicAttack_OnAirHandler onAirHandler;
    private bool resetting = false;


    public List<BasicAttack> airAttacks = new List<BasicAttack>();

    [Header("OnGUI properties")] 
    [SerializeField] private float labelX = 0f;
    [SerializeField] private float labelY = 0f;
    [SerializeField] private float labelWidth = 0f;
    [SerializeField] private float labelHeight = 0f;

    private void Awake() {
        player = Player.Instance;
        playerInfo = player.playerInfo;
        firstAuto = basicAttack;
        basicAttack.CanUse = true;
        onAirHandler = BasicAttack_OnAirHandler.Instance;
    }

    private void Update() {
        if (GameStatus.IgnoreInputs())
            return;
        
        var pressedAttack = Input.GetKeyDown(KeyCode.J) || Input.GetMouseButtonDown(0);

        ResetAuto();
        GameStatus.IsAttacking = PreventPlayerMovementTimer();

        if (pressedAttack)
            AutoAttackHandler(basicAttack);
        
    }

    public void AutoAttackHandler (BasicAttack basicAttack)
    {
        // Debug.Log($"Handler {basicAttack?.name}");
        if (resetting)
            return;
        
        if (!player.OnGround && basicAttack != null)
        {
            // if (basicAttack.airNode)
            //     basicAttack.airNode.CanUse = true;

            var pressingS = Input.GetKey(KeyCode.S) && basicAttack.airNode != null;
            DebugInput.AddInput(pressingS ? "S+J" : "J", basicAttack.name);

            Debug.Log($"Changing {basicAttack.name} to {(pressingS ? basicAttack.airNode?.rightNode?.name : basicAttack.airNode?.name)} - Air");
            this.basicAttack = pressingS ? (basicAttack.rightNode == null ? basicAttack.airNode.rightNode : basicAttack.rightNode) : basicAttack.airNode;
            this.basicAttack.CanUse = true;

            onAirHandler.AutoAttackHandler(this.basicAttack);
            return;
        }

        if (!basicAttack || !basicAttack.CanUse || nextAutoCoroutine != null) 
            return;
        
        if (!HasStamina(basicAttack.StaminaCost))
            return;

        timer -= timer;
        goRight = false;
        
        InstantiateBasicAttack();
        
        
        OnHit?.Invoke();

        NextAutoReset (basicAttack.rightNode ? basicAttack.rightNode.ResetTime : (basicAttack.leftNode ? basicAttack.leftNode.ResetTime : 1.5f));
        WaitAutoAttack(basicAttack.IsSinglePath ? 0 : basicAttack.WaitTime);

        // Antes de mudar para o próximo AA
        basicAttack.CanUse = false;
        var animationLength = basicAttack.AnimationLength;
        
        var pressingW = Input.GetKey(KeyCode.W) && basicAttack.rightNode != null;
        // Debug.Log($"{basicAttack.name} - {Input.GetKey(KeyCode.W)} {basicAttack.rightNode != null} {pressingW}");

        DebugInput.AddInput(pressingW ? "W+J" : "J", basicAttack.name);

        this.waitAuto = basicAttack?.waitNode;
        this.basicAttack = pressingW ? basicAttack.rightNode : basicAttack.leftNode;
        

        if (basicAttack != null){
            nextAutoCoroutine = StartCoroutine(LetNextAutoGoOff(animationLength, this.basicAttack));
        }
        
        PreventPlayerMovement();
        
        
        if (this.basicAttack) 
            return;
        
        StartCoroutine(ResetCombo (animationLength));
    }

    public void AddOnHitEffect (Action action){
        OnHit += action;
    }

    public void AddOnHitEffect (Action<Character> action){
        OnHitCharacter += action;
    }

    private void InstantiateBasicAttack()
    {
        var trans = transform;
        var position = trans.position;
        pos = new Vector3(position.x, position.y, basicAttack.transform.position.z);
        var g = Instantiate(basicAttack.gameObject, pos, basicAttack.transform.rotation);
        if(basicAttack.SetPlayerAsParent)
            g.transform.SetParent(player.transform);
    }

    public void PreventPlayerMovement()
    {
        attackMovementTimer -= attackMovementTimer;
        attackMovementResetTimer = 0.2f;
    }

    private bool PreventPlayerMovementTimer()
    {
        if (!(attackMovementTimer < attackMovementResetTimer)) 
            return false;
        
        attackMovementTimer += Time.deltaTime;
        return true;

    }

    public bool HasStamina (float staminaCost)
    {
        if (!(playerInfo.Stamina >= staminaCost))
        {
            Debug.Log("Not enough stamina");
            return false;
        }
        
        playerInfo.Stamina -= staminaCost;
        return true;

    }

    public void NextAutoReset (float nextAutoTimer) {
        resetTimer = nextAutoTimer;
    }

    public void WaitAutoAttack (float waitTimer){
        waitAutoTimer = waitTimer;
    }


    private bool ResetAuto(){
        if (timer <= resetTimer){
            timer += Time.deltaTime;
            if (waitAutoTimer <= 0 || goRight) 
                return true;

            if (timer < waitAutoTimer)
                return true;

            Debug.Log($"Changing {basicAttack.name} to {waitAuto?.name}");
            basicAttack = waitAuto ?? firstAuto;
            basicAttack.CanUse = true;
            goRight = true;
            return true;
        }else {
            OnResetAuto ();
            return false;
        }
        
    }

    public IEnumerator LetNextAutoGoOff (float duration, BasicAttack nextAA){
        GameStatus.IgnoreCommands(true, this);
        GameStatus.IsAttacking = true;
            
        yield return new WaitForSeconds(duration);

        if (nextAA != null)
        {
            nextAA.CanUse = true;
            // Debug.Log($"Now {nextAA.name} can be used.");
        }
        GameStatus.IsAttacking = false;
        nextAutoCoroutine = null;
        GameStatus.IgnoreCommands(false, this);
    }
    
    private void OnResetAuto (){
        if (basicAttack != null && basicAttack == firstAuto)
            return;
        
        // Debug.Log("Resetting by timer");
        timer = 0f;
        goRight = false;
        resetTimer = 0f;
        waitAutoTimer = 0f;
        DebugInput.AddInput("----", "");
        if (basicAttack){
            basicAttack.CanUse = false;
        }
        
        basicAttack = firstAuto;
        basicAttack.CanUse = true;
        waitAuto = null;

        airAttacks = new List<BasicAttack>();
    }

    public IEnumerator ResetCombo (float duration)
    {
        resetting = true;
        DebugInput.AddInput("----", "");
        yield return new WaitForSeconds(duration);
        Debug.Log($"{basicAttack?.name} Resetting combo");
        basicAttack = firstAuto;
        basicAttack.CanUse = true;

        yield return new WaitUntil( () => player.OnGround);
        resetting = false;

        airAttacks = new List<BasicAttack>();
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(labelX, labelY, labelWidth, labelHeight), $"isAttacking: {GameStatus.IsAttacking}");
        GUI.Label(new Rect(labelX, labelY + 20, labelWidth, labelHeight), $"Ignore: {GameStatus.IsIgnoringCommands}");
        var t = (attackMovementResetTimer - attackMovementTimer).ToString(CultureInfo.InvariantCulture);
        GUI.Label(new Rect(labelX, labelY + 40, labelWidth, labelHeight), $"Prev timer: {t}");
        var t1 = (resetTimer - timer).ToString(CultureInfo.InvariantCulture);
        GUI.Label(new Rect(labelX, labelY + 60, labelWidth, labelHeight), $"AA timer: {t1}");
    }
}
