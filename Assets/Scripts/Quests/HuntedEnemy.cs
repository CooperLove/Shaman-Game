using UnityEngine;
using System;

[System.Serializable]
public class Wanted {
    public Info goal;
    public int quantity = 0;
    public int quantityToGet;
    private Quest quest;

    public int Quantity { get => quantity; set => quantity = Mathf.Clamp(value, 0, quantityToGet); }

    ///<summary>Adiciona os eventos necessários para incrementar a quantidade de inimigos mortos e atualizar a UI das missões</summary>
    public void Initialize (Quest q) {
        goal.IncreaseQuantity += OnIncreaseQuantity;
        goal.IncreaseQuantity += q.UpdateProgress;
        quest = q;
    }
    public void OnIncreaseQuantity () {
        Quantity++;
        if (Quantity >= quantityToGet){
            goal.IncreaseQuantity -= OnIncreaseQuantity;
            if (quest != null)
                goal.IncreaseQuantity -= quest.UpdateProgress;
        }
    }
}
