using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public enum TimeType {Scaled, Unscaled};
    public TimeType timeType;
    public float destroyTime;
    private float timer = 0;

    private void Update() {
        switch (timeType)
        {
            case TimeType.Scaled:
                timer += Time.deltaTime;
                break;
            case TimeType.Unscaled:
                timer += Time.unscaledDeltaTime;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (timer >= destroyTime)
            Destroy(gameObject);
    }

    public void ResetTimer() => timer -= timer;
}
