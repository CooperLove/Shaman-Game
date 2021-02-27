using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DOT_Test : MonoBehaviour
{
    public bool doDamage = true;
    [SerializeField] private float tickInterval = 0.25f;
    private Player player;
    private Coroutine coroutine;
    private void Start() {
        player = Player.Instance;
        coroutine = StartCoroutine(DOT());
    }

    private void Update() {
        if (coroutine == null && doDamage)
            coroutine = StartCoroutine(DOT());
    }

    private IEnumerator DOT (){
        while (doDamage){
            player.TakeDamage (10, ignoreDamage: player.IsRolling);
            yield return new WaitForSeconds(tickInterval);
        }

        coroutine = null;
    } 
}
