using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbingScript : MonoBehaviour
{
    [SerializeField]
    private float _bobbingHeightRange = 1.0f;
    [SerializeField]
    [Tooltip("Distance covered per second along X axis of Perlin plane.")]
    float _xScaleSpeed = 1.0f;

    private float _originalY = 0;
    private float _originalZRot = 0;
    private Transform _transform;
    private float _horizontalInput = 0f;
    private float _horizontalVelocity = 0f;
    // Start is called before the first frame update
    void Start()
    {
        _originalY = transform.position.y;
        _originalZRot = transform.rotation.z;
    }

    // Update is called once per frame
    void Update()
    {
        float height = _bobbingHeightRange * Mathf.PerlinNoise(Time.time * _xScaleSpeed, 0.0f); 
        Vector3 pos = transform.position;
        Quaternion rot = transform.rotation;
        pos.y = _originalY + height;
        rot.z = _originalZRot + height;
        transform.position = pos;
        transform.rotation = rot;
    }
}
