using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Purgatory.Player
{

    [RequireComponent(typeof(Transform))]
    public class BoatController : MonoBehaviour
    {
        [SerializeField]
        private float _speed = 1f;
        [SerializeField]
        private float _deceleration = 0.9f;
        [SerializeField]
        private float _minimumLeft = -12f;
        [SerializeField]
        private float _maximumLeft = 12f;
        [SerializeField]
        private float _yawMaxRotation = 9f;
        [SerializeField]
        private float _maximumHorizontalRotation = 6f;
        [SerializeField]
        private float _bobbingHeightRange = 1.0f;
        [SerializeField]
        [Tooltip("Distance covered per second along X axis of Perlin plane.")]
        float _xScaleSpeed = 1.0f;

        private float _originalY = 0;
        private Transform _transform;
        private float _horizontalInput = 0f;
        private float _horizontalVelocity = 0f;

        void Start()
        {
            _transform = GetComponent<Transform>();
            _originalY = _transform.position.y;
        }

        void Update()
        {
            _horizontalInput = Input.GetAxis("Horizontal");
        }

        private void FixedUpdate()
        {
            _horizontalVelocity += _speed * _horizontalInput * Time.fixedDeltaTime;

            if ((_horizontalVelocity > 0 && transform.localPosition.x < _maximumLeft) || (_horizontalVelocity < 0 && transform.localPosition.x > _minimumLeft))
                _transform.position += new Vector3(_horizontalVelocity, 0, 0);

            _horizontalVelocity *= _deceleration;

            _transform.localEulerAngles = new Vector3(_yawMaxRotation * _horizontalInput, _maximumHorizontalRotation * _horizontalInput, 0);

            float height = _bobbingHeightRange * Mathf.PerlinNoise(Time.time * _xScaleSpeed, 0.0f);
            Vector3 pos = transform.position;
            pos.y = _originalY + height;
            transform.position = pos;
        }
    }

}
