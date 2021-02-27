using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [Tooltip("Ignora todos os comandos (inputs)")] private bool ignoreCommands = false;
    [Tooltip("Mult. da força de pulo.")] protected float jumpForce = 75f;
    [Tooltip("Mult. de pulo curto")] protected float lowJumpMultiplier = 15f;
    [Tooltip("Mult. de queda durante o pulo")] protected float fallMultiplier = -12f;
    [Tooltip("Velocidade de movimento")] private float velocity = 18f;
    private Image ccDurationBar;
    private Rigidbody2D _rb;
    private Transform _transform;

    private bool onGround;
    [SerializeField] private bool isStuned = false;
    private bool isBlocking = false;

    protected GameObject dmgText = null;
    protected GameObject hitFX = null;
    
    
    private void Awake() {
        dmgText = Resources.Load ("Prefabs/Combat/DamageText") as GameObject;
        hitFX = Resources.Load($"Prefabs/Combat/Hit FX/Basic Hit FX") as GameObject;
    }

    public abstract void TakeDamage (int h);
    public abstract void TakeDamage (int h, bool showHitFX = true, bool ignoreDamage = false);
    public abstract void OnBeingHit (Vector3 offset, float duration, bool stun);


    public Rigidbody2D Rb { get => _rb; set => _rb = value; }
    private GameObject DmgText { get => dmgText; set => dmgText = value; }
    public bool IgnoreCommands { get => ignoreCommands; set => ignoreCommands = value; }
    public bool OnGround { get => onGround;  set => onGround = value; }
    public bool IsStuned { get => isStuned; set => isStuned = value; }
    public float Velocity { get => velocity; set => velocity = value; }
    public Image CcDurationBar { get => ccDurationBar; set => ccDurationBar = value; }
    public bool IsBlocking { get => isBlocking; set => isBlocking = value; }
    public Transform Transform { get => _transform; set => _transform = value; }
}
