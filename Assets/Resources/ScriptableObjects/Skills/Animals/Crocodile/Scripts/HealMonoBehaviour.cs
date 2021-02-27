using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealMonoBehaviour : MonoBehaviour
{
    private GameObject heal;
    private Transform healPos;

    private void Start() {
        heal = Resources.Load("Prefabs/Combat/Wolf/Wolf - Heal") as GameObject;
        healPos = Player.Instance.transform;
    }
    public void Heal (){
        if (!heal) 
            return;
            
        var h = Instantiate (heal, healPos.position, heal.transform.rotation);
        h.TryGetComponent(out ParticleSystem ps);
        if (ps){
            ps.Play();
            //Debug.Log("?? heal ??");
        }
        h.transform.SetParent(Player.Instance.transform);
        //h.transform.localPosition = new Vector3 (0, -0.35f, 0);
        h.transform.localScale = new Vector3 (1,1,1);
        //h.transform.GetChild(0).transform.localScale = new Vector3 (0.3f, 0.4f, 1);
        Destroy(h, 3f);
    }
}
