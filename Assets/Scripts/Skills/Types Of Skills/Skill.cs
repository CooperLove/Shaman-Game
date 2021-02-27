using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Skill : ScriptableObject
{
    [SerializeField] private Sprite sprite;                // Arte da skill
    [SerializeField] private string skillName;           // Descrição da skill
    [SerializeField, TextArea(1,5)] private string description;           // Descrição da skill
    [SerializeField] private int levelToLearn;             // Nivel minimo para aprender a skill
    [SerializeField] private bool learned;                  // Se a skill ja foi aprendida

    protected Player player;
    protected PlayerInfo playerInfo;

    private void Awake() {
        Initialize();
    }

    protected void Initialize (){
        player = Player.Instance;
        playerInfo = Player.Instance.playerInfo;
    }

#region Properties
    public bool Learned { get => learned; protected set => learned = value; }
    public string Description { get => description; protected set => description = value; }
    public Sprite Sprite { get => sprite; protected set => sprite = value; }
    public string SkillName { get => skillName; protected set => skillName = value; }
    public int LevelToLearn { get => levelToLearn; protected set => levelToLearn = value; }

#endregion

    public abstract void OnLearn ();

}
