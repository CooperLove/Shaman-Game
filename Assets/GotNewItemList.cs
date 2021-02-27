using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GotNewItemList : MonoBehaviour
{
    private RectTransform rectTransform;
    private Animator animator;
    public float delay = 0.5f;
    private new Coroutine animation = null;
    private Queue<Transform> buffer = new Queue<Transform>();
    [SerializeField] private bool canRunNextAnimation = false;
    [SerializeField] private Vector3 position = new Vector3();
    private int num = 0;
    
    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void Run(Transform t)
    {
        t.gameObject.SetActive(false);
        t.name += "" + num++;
        buffer.Enqueue(t);

        if (animation != null)
            return;

        animation = StartCoroutine(Setup(t));
    }

    private IEnumerator Setup (Transform t)
    {
        // Enquanto houver quests para serem adicionadas repete a animação
        while (buffer.Count > 0)
        {
            canRunNextAnimation = false;
            StartCoroutine(PlayListAnimation(buffer.Dequeue()));
            yield return new WaitUntil(() => canRunNextAnimation);
        }
        
        animation = null;
    }

    private IEnumerator PlayListAnimation (Transform t)
    {
        // Reseta o animator e a animação
        animator.enabled = true;
        animator.Play("Got New Item List", -1, 0f);
        
        // Faz com que seja o proxímo da lista a aparecer
        t.SetSiblingIndex(0);
        
        // Aguarda a animação terminar
        yield return new WaitForSeconds(delay);
        
        t.gameObject.SetActive(true);
        animator.enabled = false;
        
        // Faz com que a lista volte para a posição inicial, garantindo o loop da animação
        rectTransform.anchoredPosition = position;

        StartCoroutine(DelayBetweenAnimations());
    }

    private IEnumerator DelayBetweenAnimations ()
    {
        yield return new WaitForSeconds(1.5f);
        // Libera a proxíma animação para ser executada
        canRunNextAnimation = true;
    }
}
