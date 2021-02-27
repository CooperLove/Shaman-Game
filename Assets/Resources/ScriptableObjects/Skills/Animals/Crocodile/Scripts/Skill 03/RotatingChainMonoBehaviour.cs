using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingChainMonoBehaviour : MonoBehaviour
{
    [SerializeField] private float radius = 22f;
    [SerializeField] private float duration = 1.25f;
    [SerializeField] private float ticks = 5;
    [SerializeField] private float timeScaleBefore = 0f;
    [SerializeField] private float pauseDuration = 0f;
    private LayerMask enemyLayer;
    // Start is called before the first frame update
    void Start()
    {
        enemyLayer = 1 << 9;
        StartCoroutine(PauseOnHit());
    }

    public IEnumerator PauseOnHit (){
        float step = duration/ticks;
        float dur = 0f;
        while (dur < duration){
            Collider2D[] enemy = Physics2D.OverlapCircleAll(transform.position, radius, enemyLayer);
            //Debug.Log($"({dur}) Enemies hit {enemy.Length} => {Time.timeScale}");
            foreach (Collider2D e in enemy)
            {
                e.GetComponent<Enemy>().TakeDamage(10);
            }
            yield return new WaitForSecondsRealtime(enemy.Length == 0 ? 0 : pauseDuration);
            Time.timeScale = timeScaleBefore;
            if (enemy.Length > 0)
                StartCoroutine(SimpleCameraShake.instance.ShakeCamera());
            yield return new WaitForSecondsRealtime(enemy.Length == 0 ? 0 : pauseDuration);
            Time.timeScale = 1;
            dur += step;
        }
        Time.timeScale = 1;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void OnDestroy() {
        Time.timeScale = 1;
    }
}
