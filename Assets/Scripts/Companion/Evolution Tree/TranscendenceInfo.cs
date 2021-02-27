using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TranscendenceInfo", menuName = "TranscendenceInfo", order = 0)]
public class TranscendenceInfo : ScriptableObject {
    [SerializeField] private Sprite sprite;
    [SerializeField] private string compName;
    [SerializeField] private Sprite compImage;
    [SerializeField, TextArea(3, 10)] private string description;

    [Header("Additional attributes points"), Space(10)]
    [SerializeField, Tooltip("Increases the companion maximum mana")] private int intelligence;
    [SerializeField, Tooltip("Decreases how much mana a spell uses")] private int intellect;
    [SerializeField, Tooltip("Increases the effect of a spell")] private int wisdom;

    //Bonus stats for the player
    [Header("Additional points for the player")]
    [SerializeField] private int additionalForce;
    [SerializeField] private int additionalInt;
    [SerializeField] private int additionalDex;
    [SerializeField] private int additionalCon;
    [SerializeField] private int additionalVigor;

    [SerializeField, Tooltip("The unique spell that each evolution has")] private GameObject uniqueSpell;

    public string CompName { get => compName; set => compName = value; }
    public Sprite CompImage { get => compImage; set => compImage = value; }
    public int Intelligence { get => intelligence; set => intelligence = value; }
    public int Intellect { get => intellect; set => intellect = value; }
    public int Wisdom { get => wisdom; set => wisdom = value; }
    public int AdditionalForce { get => additionalForce; set => additionalForce = value; }
    public int AdditionalInt { get => additionalInt; set => additionalInt = value; }
    public int AdditionalDex { get => additionalDex; set => additionalDex = value; }
    public int AdditionalCon { get => additionalCon; set => additionalCon = value; }
    public int AdditionalVigor { get => additionalVigor; set => additionalVigor = value; }
    public string Description { get => description; set => description = value; }
    public Sprite Sprite { get => sprite; set => sprite = value; }
}

