using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mark : MonoBehaviour
{
    public Transform marks;
    private int qtdMarks;
    [SerializeField] private float energy;
    public bool canUseMark = true;

    public int QtdMarks { get => qtdMarks; set => qtdMarks = Mathf.Clamp(value, 0, 5); }
    public float Energy { get => energy; set => energy = Mathf.Clamp(value, 0, 100); }

    // Start is called before the first frame update
    void Start()
    {
        canUseMark = false;
    }

    public void Use () {
        if (!canUseMark)
            return;
        QtdMarks--;
        marks.GetChild(QtdMarks).gameObject.SetActive(false);
        canUseMark = qtdMarks == 0 ? false : true;
        Debug.Log($"Used a mark. Marks remain {QtdMarks}");
    }

    public void Gain () {
        if (qtdMarks >= 5)
            return;

        canUseMark = true;
        marks.GetChild(QtdMarks).gameObject.SetActive(true);
        QtdMarks++;
        Debug.Log($"Gained a mark. Marks remain {QtdMarks}");
    }
    public bool Gain (float qtd) {
        if (qtdMarks == 5)
            return false;
        Energy += qtd;
        if (Energy == 100){
            Gain();
            Energy = 0;
            return true;
        }
        return false;
    }
}
