using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatterAttack : MonoBehaviour
{
    public int numBubbles = 3;
    [SerializeField] private List<WaterSpawn> prefab = new List<WaterSpawn>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numBubbles; i++)
        {
            StartCoroutine (Spawn(prefab[i]));
        }
    }

    private void OnEnable() {
        for (int i = 0; i < numBubbles; i++)
        {
            prefab[i].prefab.SetActive(false);
            StartCoroutine (Spawn(prefab[i]));
        }
    }

    private IEnumerator Spawn (WaterSpawn bubble){
        Debug.Log($"Waiting {bubble.spawnDelay} for {bubble.prefab?.name}");
        yield return new WaitForSeconds(bubble.spawnDelay);
        bubble.prefab.SetActive(true);
    }
}

[System.Serializable]
public class WaterSpawn {
    public GameObject prefab = null;
    public float spawnDelay = 0.0f;
}
