using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class test : MonoBehaviour
{
    float accelerometerUpdateInterval = 1.0f / 60.0f;
    float lowPassKernelWidthInSeconds = 1.0f;
    float shakeDetectionThreshold = 0.8f;

    float lowPassFilterFactor;
    Vector3 lowPassValue;

    void Start()
    {
        lowPassFilterFactor = accelerometerUpdateInterval / lowPassKernelWidthInSeconds;
        shakeDetectionThreshold *= shakeDetectionThreshold;
        lowPassValue = Input.acceleration;
    }

    void Update()
    {
        Vector3 acceleration = Input.acceleration;
        lowPassValue = Vector3.Lerp(lowPassValue, acceleration, lowPassFilterFactor);
        Vector3 deltaAcceleration = acceleration - lowPassValue;

        if (deltaAcceleration.sqrMagnitude >= shakeDetectionThreshold)
        {
            // Perform your "shaking actions" here. If necessary, add suitable
            // guards in the if check above to avoid redundant handling during
            // the same shake (e.g. a minimum refractory period).
            Debug.Log(true);
        }
        else Debug.Log(false);
    }
}
