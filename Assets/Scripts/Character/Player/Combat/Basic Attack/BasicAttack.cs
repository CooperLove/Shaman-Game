using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BasicAttack : MonoBehaviour
{   
    [SerializeField] private bool canUse = false;
    [SerializeField] private bool setPlayerAsParent = false;
    [SerializeField] private bool knockUp = false;
    [SerializeField] private bool isSinglePath = false;
    [SerializeField] private float staminaCost = 0f;
    [SerializeField] private float animationLength = 0f;
    [SerializeField] private float airDuration = 0f;
    [SerializeField] private float waitTime = 0f;
    [SerializeField] private float resetTime = 1f;
    public BasicAttack airNode = null;
    public BasicAttack leftNode = null;
    public BasicAttack rightNode = null;
    public BasicAttack waitNode = null;
    

    public float StaminaCost { get => staminaCost; set => staminaCost = value; }
    public bool KnockUp { get => knockUp; set => knockUp = value; }
    public float WaitTime => waitTime;
    public float ResetTime => resetTime;
    public bool IsSinglePath => isSinglePath;
    public float AirDuration { get => airDuration; set => airDuration = value; }
    public bool SetPlayerAsParent { get => setPlayerAsParent; set => setPlayerAsParent = value; }
    public bool CanUse { get => canUse; set => canUse = value; }
    public float AnimationLength { get => animationLength; set => animationLength = value; }
}
