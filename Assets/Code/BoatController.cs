using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Transform))]
public class BoatController : MonoBehaviour
{
    public float Speed = 1f;
    public float MinimumLeft = -12f;
    public float MaximumLeft = 12f;
    public float YawMaxRotation = 9f;
    public float MaximumHorizontalRotation = 6f;
    private Transform _transform;
    private float _horizontalInput = 0f;

    void Start()
    {
        _transform = GetComponent<Transform>();
    }

    void Update()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        if (_horizontalInput > 0 && transform.localPosition.z < MaximumLeft)
            _transform.position += new Vector3(Speed * _horizontalInput * Time.fixedDeltaTime, 0, 0);
        else if (_horizontalInput < 0 && transform.localPosition.z > MinimumLeft)
            _transform.position += new Vector3(Speed * _horizontalInput * Time.fixedDeltaTime, 0, 0);

        _transform.localEulerAngles = new Vector3(YawMaxRotation * _horizontalInput, MaximumHorizontalRotation * _horizontalInput, 0);

    }
}
