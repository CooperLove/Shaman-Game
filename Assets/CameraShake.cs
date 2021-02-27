using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public bool shakeOnEnable = false;
    public float startDelay = 0.0f;
    public float ShakeDuration = 0.3f;          // Time the Camera Shake effect will last
    public float ShakeAmplitude = 1.2f;         // Cinemachine Noise Profile Parameter
    public float ShakeFrequency = 2.0f;         // Cinemachine Noise Profile Parameter
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Shake());
    }

    private IEnumerator Shake (){
        yield return new WaitForSeconds(startDelay);
        //Debug.Log($"Shake on {name}");
        
        StartCoroutine(SimpleCameraShake.instance.ShakeCamera(ShakeAmplitude, ShakeFrequency, ShakeDuration));
    }

    private void OnEnable() {
        if (shakeOnEnable)
            StartCoroutine(Shake());
    }
}
