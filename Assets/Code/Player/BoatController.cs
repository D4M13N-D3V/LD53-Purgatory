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
        public static BoatController instance;

        public BoatController()
        {
             instance = this;
        }

        [SerializeField]
        private float _minimumLeft = -12f;
        [SerializeField]
        private float _maximumLeft = 12f;
        [SerializeField]
        private float _yawMaxRotation = 9f;
        [SerializeField]
        private float _maximumHorizontalRotation = 6f;
        [HideInInspector]
        public bool Dashing = false;


        [SerializeField]
        public float Speed = 1f;
        [SerializeField]
        public float Deceleration = 0.9f;
        [SerializeField]
        public float DashMultiplier = 3f;
        [SerializeField]
        public float DashLength = 1f;
        [SerializeField]
        public float DashCooldown = 1f;


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


        void Start()
        {
            _transform = GetComponent<Transform>();
            _originalY = _transform.position.y;
            _moveAction = actions.FindActionMap("gameplay").FindAction("move", true);
            actions.FindActionMap("gameplay").FindAction("dash").performed += Dash;
            _originalSpeed = Speed;
        }

        private void Dash(InputAction.CallbackContext obj)
        {
            if(!Dashing)
                StartCoroutine(DashCoroutine());
        }

        IEnumerator DashCoroutine()
        {
            Dashing = true;
            _originalSpeed = Speed;
            Speed = Speed * DashMultiplier;
            yield return new WaitForSeconds(DashLength);
            Speed = _originalSpeed;
            yield return new WaitForSeconds(DashCooldown-DashLength);
            Dashing = false;
        }

        void Update()
        {
            _horizontalInput = _moveAction.ReadValue<Vector2>().x * -1;
        }

        private void FixedUpdate()
        {            
            _waveDisruptionObject.transform.localPosition = new Vector3(Mathf.Lerp(_waveDisruptionObject.transform.localPosition.x,-0.75f+(_horizontalInput*2), Time.deltaTime * 2), _waveDisruptionObject.transform.localPosition.y, _waveDisruptionObject.transform.localPosition.z);

            _targetVelocity = Speed * _horizontalInput;
            _horizontalVelocity = Mathf.Lerp(_horizontalVelocity, _targetVelocity, Time.fixedDeltaTime * (Dashing ? Speed / _originalSpeed : 1) / Deceleration);

            if ((_horizontalVelocity > 0 && transform.position.x < _maximumLeft) || (_horizontalVelocity < 0 && transform.position.x > _minimumLeft))
                _transform.position += new Vector3(_horizontalVelocity, 0, 0);

            _horizontalVelocity *= Deceleration;

            _transform.localEulerAngles = new Vector3(_transform.localEulerAngles.x, _transform.localEulerAngles.y, Mathf.LerpAngle(transform.localEulerAngles.z, _maximumHorizontalRotation * _horizontalInput * -1, Time.deltaTime * 2));

        }
    }

}
