using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{   
    [SerializeField] private new string name;
    protected bool canInteract = false;
    public string Name { get => name; set => name = value; }

    /// <summary>Ações que devem ser executadas assim que o player fala com o NPC.</summary>
    public abstract void OnBeginInteraction ();
    public abstract void OnEndInteraction ();

    // private void Update() {
    //     if (Input.GetKeyDown(KeyCode.E) && canInteract){
    //         OnBeginInteraction();
    //         canInteract = false;
    //     }
    // }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag.Equals("Player")){
            //Debug.Log($"Pode interagir com {name}");
            canInteract = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag.Equals("Player")){
            //Debug.Log($"Não pode interagir com {name}");
            canInteract = false;
        }
    }
}
