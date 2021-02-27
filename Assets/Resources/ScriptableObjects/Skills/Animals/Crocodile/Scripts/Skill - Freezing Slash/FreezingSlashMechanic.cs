using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezingSlashMechanic : MonoBehaviour
{
    private ParticleSystem ps = null;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponentInChildren<ParticleSystem>(true);
        if (!Player.Instance.IsFacingRight){
            transform.localScale = new Vector3(-1, 1, 1);
            if (ps)
                ps.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

      
    public void StopParticles (){
        ps?.Stop();
        ps?.transform.GetChild(0)?.GetComponent<ParticleSystem>()?.Stop();
        Destroy(gameObject, 2f);
    }
}
