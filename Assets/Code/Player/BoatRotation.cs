using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatRotation : MonoBehaviour
{
    [SerializeField] private float waveAmplitude = 1.0f;
    [SerializeField] private float waveFrequency = 1.0f;
    [SerializeField] private float wavePhaseOffset = 0.0f;
    [SerializeField] private Vector3 waveDirection = Vector3.forward;

    private Quaternion initialRotation;
    private Purgatory.Player.BoatController boatController;

    void Start()
    {
        initialRotation = transform.localRotation;
        boatController = GetComponent<Purgatory.Player.BoatController>();
    }

    void Update()
    {
        float wavePhase = waveFrequency * Time.time + wavePhaseOffset;
        float rotationAngle = Mathf.Atan2(waveAmplitude * waveFrequency * Mathf.Cos(wavePhase), 1);
        Quaternion waveRotation = Quaternion.AngleAxis(rotationAngle * Mathf.Rad2Deg, Vector3.Cross(Vector3.up, waveDirection));

        if (boatController != null)
        {
            Quaternion boatControllerRotation = Quaternion.Euler(boatController.transform.localEulerAngles);
            transform.localRotation = initialRotation * waveRotation * boatControllerRotation;
        }
        else
        {
            transform.localRotation = initialRotation * waveRotation;
        }
    }
}
