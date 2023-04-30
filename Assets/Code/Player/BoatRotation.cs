using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatRotation : MonoBehaviour
{
    [SerializeField] private float waveAmplitude = 1.0f;
    [SerializeField] private float waveFrequency = 1.0f;
    [SerializeField] private float wavePhaseOffset = 0.0f;
    [SerializeField] private Vector3 waveDirection = Vector3.forward;
    
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start()
    {
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        float wavePhase = waveFrequency * Time.time + wavePhaseOffset;
        float waveHeight = waveAmplitude * Mathf.Sin(wavePhase);
        Vector3 wavePosition = waveHeight * waveDirection.normalized;
        transform.localPosition = initialPosition + wavePosition;

        float rotationAngle = Mathf.Atan2(waveAmplitude * waveFrequency * Mathf.Cos(wavePhase), 1);
        Quaternion waveRotation = Quaternion.AngleAxis(rotationAngle * Mathf.Rad2Deg, Vector3.Cross(Vector3.up, waveDirection));
        transform.localRotation = initialRotation * waveRotation;
    }
}
