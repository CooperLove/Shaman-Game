using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    public RectTransform ps1;
    public RectTransform[] path1;
    public float value, time;
    public int noAtual1;
    public ParticleSystem partSystem, g;
    public Transform t;
    // Start is called before the first frame update
    void Start()
    {
        if (path1.Length > 0){
            ps1.localPosition = path1[0].localPosition;
            noAtual1 = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
       // FollowPath ();
        //SetupCorners ();
        if (path1.Length == 0)
            return;
        float d = Vector3.Distance(path1[noAtual1].localPosition, ps1.localPosition);
        if (d > value){
            ps1.localPosition = Vector3.MoveTowards (ps1.localPosition, path1[noAtual1].localPosition, time * Time.deltaTime);
        }else {
            g = (ParticleSystem) Instantiate(partSystem, path1[noAtual1].localPosition, Quaternion.identity);
            g.transform.SetParent(t);
            g.tag = "Destroy";
            g.transform.localScale = Vector3.one;
            g.transform.localPosition = path1[noAtual1].localPosition;
            if (noAtual1 + 1 < path1.Length) noAtual1++;
            else noAtual1 = 0;
        }
    }

    

}
