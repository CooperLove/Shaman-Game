using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HighlightAnimation : MonoBehaviour
{
    public enum Corner
    {
        upperRight, upperLeft, bottomLeft, bottomRight
    }

    public RectTransform ps1;
    public Corner dropdown;
    public RectTransform[] path1;
    public float distance, velocity;
    public int noAtual1;
    // Start is called before the first frame update
    void Start()
    {
        SetupCorners ();
        ps1.localPosition = path1[(int)dropdown].localPosition;
        noAtual1 = (int)dropdown + 1 >= path1.Length ? 0 : (int)dropdown + 1;
    }

    // Update is called once per frame
    void Update()
    {
       // FollowPath ();
        //SetupCorners ();
        float d = Vector3.Distance(path1[noAtual1].localPosition, ps1.localPosition);
        if (d > distance){
            ps1.localPosition = Vector3.MoveTowards (ps1.localPosition, path1[noAtual1].localPosition, velocity * Time.deltaTime);
        }else {
            if (noAtual1 + 1 < path1.Length) noAtual1++;
            else noAtual1 = 0;
        }
    }

    public void SetupCorners (){
        RectTransform r = transform.parent.GetComponent<RectTransform>();
        //Debug.Log($"Rect {r.sizeDelta}");
        path1[0].localPosition = new Vector3 (r.sizeDelta.x/2, r.sizeDelta.y/2, 0);
        path1[1].localPosition = new Vector3 (-r.sizeDelta.x/2, r.sizeDelta.y/2, 0);
        path1[2].localPosition = new Vector3 (-r.sizeDelta.x/2, -r.sizeDelta.y/2, 0);
        path1[3].localPosition = new Vector3 (r.sizeDelta.x/2, -r.sizeDelta.y/2, 0);
    }
}
