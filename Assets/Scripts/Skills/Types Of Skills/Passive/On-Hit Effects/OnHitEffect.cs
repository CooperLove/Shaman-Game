using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OnHitEffect : UpgradableSkill
{
    private void Awake() {
        base.Initialize();
        if (Learned)
            BasicAttack_Handler.Instance.AddOnHitEffect ((Action) OnHit);
    }

    /// <summary> Adiciona aos ataques básicos, os efeitos desta habilidade. </summary>
    public override void OnLearn()
    {
        if (Learned)
            return;

        BasicAttack_Handler.Instance.AddOnHitEffect ((Action) OnHit);
        AddOneTimeEffects();

        Learned = true;
    }

    /// <summary> Esse método é basicamente, uma passiva que é utilizada apenas uma vez.
    /// <para> Algumas skills possuem uma parte que não é ativada sempre que ocorrer um ataque. </para>
    /// <example> Ex: Ataques regeneram 2.5% da vida máxima, porém o dano de cada ataque é reduzido em 10% </example>
    /// <para> O dano reduzido dos ataques só precisa ser modificado uma única vez. </para>
    /// </summary>
    protected abstract void AddOneTimeEffects ();

    /// <summary> Efeitos que serão aplicados sempre que um ataque básico for efetuado. </summary>
    public abstract void OnHit ();
    public abstract void OnHit (Character info);
}
