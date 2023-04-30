using Purgatory.Impl;
using Purgatory.Interfaces;
using Purgatory.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Purgatory.Enemy
{
    public class EnemyController : TargetableAttacker
    {
        [SerializeField]
        public GameObject _enemySoul;
        [SerializeField]
        private bool _isRanged = true;
        [SerializeField]
        private bool _isRusher = false;
        [SerializeField]
        private float _speed = 1f;
        [SerializeField]
        private float _turnSpeed = 4f;
        [SerializeField]
        private float _targetRefreshRate = 2f;
        [SerializeField]
        private int _rusherDamage = 10;
        [SerializeField]
        private Vector3 _cachedLocation;

        public EnemyController() : base()
        {
        }
        public override void DeathLogic()
        {
            GameObject.Instantiate(_enemySoul, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
        private IEnumerator GetTargetPosition()
        {
            if (_targetTransform != null)
                _cachedLocation = _targetTransform.position;
            yield return new WaitForSeconds(_targetRefreshRate);
            StartCoroutine(GetTargetPosition());
        }

        public override GameObject GetTarget()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, AttackRange);
            var target = hitColliders.Where(x => x.GetComponent<PlayerController>() != null).OrderBy(x => Guid.NewGuid()).FirstOrDefault();

            if (target != null)
                Debug.Log($"Enemy {this.gameObject.name} targeting player");

            return target?.gameObject;
        }

        public override void LaunchProjectile()
        {
            if (_isRanged)
            {
                var rotation = Quaternion.LookRotation(_targetTransform.position - transform.position);
                var newProject = GameObject.Instantiate(Projectile, transform.position + (transform.forward * 1.5f), rotation);
                Debug.Log("Enemy projectile launched!");
            }
        }

        private void Awake()
        {

            StartCoroutine(GetTargetPosition());
        }

        // Update is called once per frame
        void Update()
        {
            if (_targetTransform != null)
            {
                Quaternion _lookRotation = Quaternion.LookRotation((_targetTransform.position - transform.position).normalized);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, _lookRotation, Time.fixedDeltaTime * _aimSpeed);

                if (_cachedLocation == null)
                    _cachedLocation = _targetTransform.position;
            }
            if (_isRusher)
            {

                transform.position = Vector3.MoveTowards(transform.position, _cachedLocation, Time.fixedDeltaTime * _speed);
            }
        }
    }
}
