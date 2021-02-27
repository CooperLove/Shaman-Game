using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPC : Interactable
{
    public Conversation conversation = null;

    /// <summary>Ações que devem ser executadas assim que a conversa com o NPC termina</summary>
    public abstract void OnEndConversation ();

    

}
