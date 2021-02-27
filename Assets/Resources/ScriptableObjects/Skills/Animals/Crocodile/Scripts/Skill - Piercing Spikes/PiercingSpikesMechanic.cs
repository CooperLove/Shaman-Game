using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercingSpikesMechanic : MonoBehaviour
{
    [SerializeField] private bool leftSide = false;
    [SerializeField] private List<GameObject> spike = new List<GameObject>();
    [SerializeField] private float startDelay = 0.5f;
    [SerializeField] private float delayBtwSpikes = 0.5f;
    [SerializeField] private float spikeDestroyTime = 1.25f;
    [SerializeField] private float xOffset = 15f;
    [SerializeField] private int numberOfSpikes = 0;
    [SerializeField] private float spaceBtwSpikes = 0f;
    [SerializeField] private float minAngle = -12.5f;
    [SerializeField] private float maxAngle = 12.5f;
    [SerializeField] private float minOffset = -9f;
    [SerializeField] private float maxOffset = 0f;

    private LayerMask groundLayer = 0;

    // Start is called before the first frame update
    void Start()
    {
        groundLayer = 1 << 10;
        leftSide = !Player.Instance.IsFacingRight;
        transform.localScale = leftSide ? new Vector3(-1,1,1) : Vector3.one;
        StartCoroutine(CreateSpikes());
    }

    private void OnEnable() {
        StartCoroutine(CreateSpikes());
    }


    private IEnumerator CreateSpikes (){
        yield return new WaitForSeconds(startDelay);
        
        var n = 0;
        var pos = new Vector3 (xOffset, 0, 0);

        while (n++ < numberOfSpikes){
            var ray = Physics2D.Raycast(transform.position + (leftSide ? -pos : pos), Vector2.down, 25f, groundLayer);
            // Debug.Log($"Tr {transform.position} pos {pos}");
            Debug.DrawRay(transform.position + (leftSide ? -pos : pos), Vector3.down * 25f, Color.magenta, 2f);
            if (ray.collider == null)
                continue;

            var index = Random.Range(0, spike.Count);
            var g = Instantiate(spike[index], pos, spike[index].transform.rotation);
            var spikeTransform = g.transform;
            spikeTransform.SetParent(transform);
            spikeTransform.rotation = Quaternion.Euler (0, 0, Random.Range(minAngle, maxAngle));
            spikeTransform.localScale = Vector3.one;
            spikeTransform.localPosition = new Vector3(pos.x, 0 + Random.Range(minOffset,maxOffset), 0);

            g.SetActive(true);
            Destroy(g, spikeDestroyTime);

            pos.x += spaceBtwSpikes;
            if (delayBtwSpikes > 0)
                yield return new WaitForSeconds(delayBtwSpikes);
            else
                yield return new WaitForEndOfFrame();
            //yield return new WaitForSeconds(delayBtwSpikes);
        }
    }
}
