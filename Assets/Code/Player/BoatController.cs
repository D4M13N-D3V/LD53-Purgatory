using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
        [Tooltip("Distance covered per second along X axis of Perlin plane.")]
        float _xScaleSpeed = 1.0f;
        [SerializeField] 
        private GameObject _waveDisruptionObject;

        private float _originalY = 0;
        private Transform _transform;
        private float _horizontalInput = 0f;
        private float _horizontalVelocity = 0f;

        [SerializeField]
        public InputActionAsset actions;
        private InputAction _moveAction;

        void Start()
        {
            _transform = GetComponent<Transform>();
            _originalY = _transform.position.y;
            _moveAction = actions.FindActionMap("gameplay").FindAction("move", true);
            actions.FindActionMap("gameplay").FindAction("dash").performed += Dash;
        }

        private void Dash(InputAction.CallbackContext obj)
        {
            throw new NotImplementedException();
        }

        void Update()
        {
            _horizontalInput = _moveAction.ReadValue<Vector2>().x * -1;
            Debug.Log(_moveAction.ReadValue<Vector2>());
        }

        private void FixedUpdate()
        {
            _horizontalVelocity += _speed * _horizontalInput * Time.fixedDeltaTime;


            _waveDisruptionObject.transform.localPosition = new Vector3(-0.75f+(_horizontalInput*2), _waveDisruptionObject.transform.localPosition.y, _waveDisruptionObject.transform.localPosition.z);

            if ((_horizontalVelocity > 0 && transform.position.x < _maximumLeft) || (_horizontalVelocity < 0 && transform.position.x > _minimumLeft))
                _transform.position += new Vector3(_horizontalVelocity, 0, 0);

            _horizontalVelocity *= _deceleration;

            _transform.localEulerAngles = new Vector3(_transform.localEulerAngles.x, _transform.localEulerAngles.y, _maximumHorizontalRotation * _horizontalInput*-1);

        }
    }

}
