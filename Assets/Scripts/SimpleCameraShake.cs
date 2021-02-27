using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class SimpleCameraShake : MonoBehaviour {

    public bool shake = false;
    [FormerlySerializedAs("ShakeDuration")] public float shakeDuration = 0.3f;          // Time the Camera Shake effect will last
    [FormerlySerializedAs("ShakeAmplitude")] public float shakeAmplitude = 1.2f;         // Cinemachine Noise Profile Parameter
    [FormerlySerializedAs("ShakeFrequency")] public float shakeFrequency = 2.0f;         // Cinemachine Noise Profile Parameter

    [SerializeField] private float ShakeElapsedTime = 0f;

    // Cinemachine Shake
    public CinemachineVirtualCamera VirtualCamera;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;

    public static SimpleCameraShake instance = null;
    private SimpleCameraShake(){
        instance = this;
    }

    // Use this for initialization
    private void Start()
    {
        //Debug.Log($"Shake in {name}");
        if (shake)
            StartCoroutine(ShakeCamera());
        //Debug.Log(transform.parent.name);
        // Get Virtual Camera Noise Profile
        if (VirtualCamera)
            virtualCameraNoise = VirtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
    }

    

    public IEnumerator ShakeCamera (){
        virtualCameraNoise.m_AmplitudeGain = 0f;
        ShakeElapsedTime = 0f;
        StartCoroutine(Reset());

        ShakeElapsedTime = shakeDuration;
        while(ShakeElapsedTime > 0)
        {
            if (!VirtualCamera || !virtualCameraNoise) 
                continue;
            // If Camera Shake effect is still playing
            // Set Cinemachine Camera Noise parameters
            virtualCameraNoise.m_AmplitudeGain = shakeAmplitude;
            virtualCameraNoise.m_FrequencyGain = shakeFrequency;

            // Update Shake Timer
            ShakeElapsedTime -= Time.unscaledDeltaTime;
            yield return new WaitForEndOfFrame();
        }
        
    }

    public IEnumerator ShakeCamera (float amplitude, float freq, float dur){
        virtualCameraNoise.m_AmplitudeGain = 0f;
        ShakeElapsedTime = 0f;
        StartCoroutine(Reset(dur));

        ShakeElapsedTime = dur;
        while(ShakeElapsedTime > 0){
            if (VirtualCamera != null && virtualCameraNoise != null)
            {
                // If Camera Shake effect is still playing
                // Set Cinemachine Camera Noise parameters
                virtualCameraNoise.m_AmplitudeGain = amplitude;
                virtualCameraNoise.m_FrequencyGain = freq;

            }   
            // Update Shake Timer
            ShakeElapsedTime -= Time.unscaledDeltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator Reset (){
        yield return new WaitForSeconds(shakeDuration);
        // If Camera Shake effect is over, reset variables
        virtualCameraNoise.m_AmplitudeGain = 0f;
        ShakeElapsedTime = 0f;
    }

    private IEnumerator Reset (float dur){
        yield return new WaitForSeconds(dur);
        // If Camera Shake effect is over, reset variables
        virtualCameraNoise.m_AmplitudeGain = 0f;
        ShakeElapsedTime = 0f;
    }
}