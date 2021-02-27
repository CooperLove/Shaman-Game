using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Dreamcatcher : MonoBehaviour
{
    [ColorUsage(true, true)]
    public Color testColor;
    public GameObject g;
    public Transform test;
    [Range(0,1)]
    public float fill;
    public Image[] feathers;
    public ParticleSystem[] particles;
    public UnityEngine.Experimental.Rendering.Universal.Light2D[] lights;
    public GameObject[] highlights;
    public Transform magicCircle;
    public float circleSpeed, increment, decrement;
    public float fillAmount, lightIntensity;
    public int value;
    void Start() {
        circleSpeed = -1;
        fillAmount = Player.Instance.playerInfo.ChargeAmount;
        foreach (Image feather in feathers)
            feather.fillAmount = 0;
        foreach (UnityEngine.Experimental.Rendering.Universal.Light2D light in lights)
            light.intensity = 0;
        foreach (ParticleSystem p in particles)
            p.Stop();
        foreach (GameObject h in highlights)
            h.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
        float t = Time.realtimeSinceStartup;
        /*
        test.localPosition = new Vector3 (0, (120 * fill) - 60, 0);
        //UpdateFillAmount();
        if (Input.GetKeyDown(KeyCode.Z)){
            StopAllCoroutines();
            StartCoroutine (SpendCharges());
        }
        if (Input.GetKeyDown(KeyCode.X)){
            StopAllCoroutines();
            StartCoroutine (FillCharges());
        }
        if (Input.GetKeyDown(KeyCode.F))
            TotalCharge();
        */

        t = Time.realtimeSinceStartup - t;
//        Debug.Log("Tempo de um update: "+t*1000);
        
    }

    public void ggg (){
        for (int i = 0; i < 360; i+=value)
        {
            Debug.Log(i+" "+Mathf.Sin(i)+" "+Mathf.Cos(i)); 
        }
    }

    public void UpdateFillAmount (){
        fillAmount = Player.Instance.playerInfo.ChargeAmount;
        magicCircle.Rotate(new Vector3(0,0,circleSpeed * fillAmount));

        if (fillAmount < feathers.Length)
            feathers[(int)fillAmount].fillAmount = fillAmount - (int)fillAmount;
        else
            foreach (Image feather in feathers)
                feather.fillAmount = 1;
            
        // Faz com que o valor das penas que estejam completamente carregadas seja 1
        for (int i = 0; i < (int)fillAmount; i++)
            if (fillAmount > 0)
                feathers[i].fillAmount = 1;

        // Faz com que o valor das penas que estejam completamente descarregadas seja 0
        for (int i = feathers.Length - 1; i > (int)fillAmount; i--)
            feathers[i].fillAmount = 0;

        // Ativa as particulas e o highlight se a pena i estiver completa
        for (int i = 0; i < feathers.Length; i++){
            if (feathers[i].fillAmount > 0.97f){
                particles[i].Play();
                highlights[i].SetActive(true);
            }
            else{
                particles[i].Stop();
                highlights[i].SetActive(false);
            }
        }

        // Ajusta a intensidade da luz na posição i, para representar o preenchimento da pena.
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].intensity = feathers[i].fillAmount * lightIntensity;
        }

        //Debug.Log("Pena "+(int)fillAmount+" "+fillAmount+" "+(fillAmount - (int)fillAmount) );
    }

    public IEnumerator SpendCharges (){
        while (Player.Instance.playerInfo.ChargeAmount > 0)
        {
            Player.Instance.playerInfo.ChargeAmount -= decrement * Time.deltaTime;
            yield return null;
        }
        Player.Instance.playerInfo.ChargeAmount = Mathf.Clamp (Player.Instance.playerInfo.ChargeAmount, 0, 5);
    }
    public IEnumerator FillCharges (){
        while (Player.Instance.playerInfo.ChargeAmount < 5)
        {
            Player.Instance.playerInfo.ChargeAmount += increment * Time.deltaTime;
            int i = (int) Player.Instance.playerInfo.ChargeAmount;

            yield return null;
        }
        Player.Instance.playerInfo.ChargeAmount = Mathf.Clamp (Player.Instance.playerInfo.ChargeAmount, 0, 5);
    }

    public void TotalCharge (){
        Player.Instance.playerInfo.ChargeAmount = 5;
    }

    public void AddCharge (){
        float value = Random.Range (0.15f, 0.35f);
        Player.Instance.playerInfo.ChargeAmount += value;
        Player.Instance.playerInfo.ChargeAmount = Mathf.Clamp (Player.Instance.playerInfo.ChargeAmount, 0, 5);
    }
    public void SubCharge (){
        float value = Random.Range (0.15f, 0.35f);
        Player.Instance.playerInfo.ChargeAmount -= value;
        Player.Instance.playerInfo.ChargeAmount = Mathf.Clamp (Player.Instance.playerInfo.ChargeAmount, 0, 5);
    }
}
