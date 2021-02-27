using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UpgradableSkill : Skill
{
    [SerializeField] private int currentLevel;             // Nivel atual da skill
    [SerializeField] private int maxLevel;                 // Nivel maximo que a skill pode atingir
    [SerializeField] private int levelIncrement;           // Nivel que será incrementado para aprender um novo nivel da skill
    [SerializeField] private int pointsPerLevel;           // Quantos pontos serão gastos para evoluir a skills
    [SerializeField] private float cooldown;               // Tempo de recarga

#region Properties
    public float Cooldown { get => cooldown; protected set => cooldown = value; }
    public int LevelIncrement { get => levelIncrement; protected set => levelIncrement = value; }
    public int MaxLevel { get => maxLevel; protected set => maxLevel = value; }
    public int CurrentLevel { get => currentLevel; protected set => currentLevel = value; }
    public int PointsPerLevel { get => pointsPerLevel; protected set => pointsPerLevel = value; }  

#endregion

    /// <summary> Método utilizado para aprimorar uma habilidade. </summary>
    public abstract void OnUpgrade ();
    
    /// <summary> Verifica se a habilidade pode ser aprimorada.
    /// </summary>
    /// <returns>
    /// <para>True: Se pode ser aprimorada </para>
    /// False: Caso contrário
    /// </returns>
    protected bool CanUpgradeSkill () {
        //Se o player nao tiver o nivel necessario ou pontos para aprender a skill, retorna.
        if (CurrentLevel >= MaxLevel || playerInfo.ActiveSkillPoints < PointsPerLevel)
            return false;

        CurrentLevel+= 1; // Aumenta o nivel atual 
        LevelToLearn += LevelIncrement; // Atualiza o nivel para aprender o proximo nivel
        player.playerInfo.ActiveSkillPoints -= PointsPerLevel; // Retira os pontos para aprender habilidades
        return true;
    }
}
