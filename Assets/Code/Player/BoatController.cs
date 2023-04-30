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
        private float _dashMultiplier = 3f;
        [SerializeField]
        private float _dashLength = 1f;
        [SerializeField]
        private float _dashCooldown = 1f;
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
        private float _targetVelocity = 0f;

        private float _originalSpeed;

        [SerializeField]
        public InputActionAsset actions;
        private InputAction _moveAction;

        private bool _dashing = false;

        void Start()
        {
            _transform = GetComponent<Transform>();
            _originalY = _transform.position.y;
            _moveAction = actions.FindActionMap("gameplay").FindAction("move", true);
            actions.FindActionMap("gameplay").FindAction("dash").performed += Dash;
            StartCoroutine(UpdateHud());
            _originalSpeed = _speed;
        }

        private IEnumerator UpdateHud()
        {
            HudController.instance.UpdateBoatStats(_speed, _originalSpeed*_dashMultiplier, _dashLength, !_dashing, _dashCooldown);
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(UpdateHud());
        }
        private void Dash(InputAction.CallbackContext obj)
        {
            if(!_dashing)
                StartCoroutine(DashCoroutine());
        }

        IEnumerator DashCoroutine()
        {
            _dashing = true;
            _originalSpeed = _speed;
            _speed = _speed * _dashMultiplier;
            yield return new WaitForSeconds(_dashLength);
            _speed = _originalSpeed;
            yield return new WaitForSeconds(_dashCooldown-_dashLength);
            _dashing = false;
        }

        void Update()
        {
            _horizontalInput = _moveAction.ReadValue<Vector2>().x * -1;
            Debug.Log(_moveAction.ReadValue<Vector2>());
        }

        private void FixedUpdate()
        {
            _waveDisruptionObject.transform.localPosition = new Vector3(Mathf.Lerp(_waveDisruptionObject.transform.localPosition.x, -0.75f + (_horizontalInput * 2), Time.deltaTime * 2), _waveDisruptionObject.transform.localPosition.y, _waveDisruptionObject.transform.localPosition.z);

            _targetVelocity = _speed * _horizontalInput;
            _horizontalVelocity = Mathf.Lerp(_horizontalVelocity, _targetVelocity, Time.fixedDeltaTime * (_dashing ? _speed / _originalSpeed : 1) / _deceleration);

            if ((_horizontalVelocity > 0 && transform.position.x < _maximumLeft) || (_horizontalVelocity < 0 && transform.position.x > _minimumLeft))
                _transform.position += new Vector3(_horizontalVelocity, 0, 0);

            _transform.localEulerAngles = new Vector3(_transform.localEulerAngles.x, _transform.localEulerAngles.y, Mathf.LerpAngle(transform.localEulerAngles.z, _maximumHorizontalRotation * _horizontalInput * -1, Time.deltaTime * 2));
        }
    }

}
