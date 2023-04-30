﻿using Purgatory.Interfaces;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Purgatory.Impl
{
    public abstract class Attacker : MonoBehaviour, IAttacker
    {
        [SerializeField]
        private bool _hidden = false;
        [SerializeField]
        private float _distanceToShowSelf = 10f;
        [SerializeField]
        private GameObject _projectile = null;
        [SerializeField]
        private float _attackIntervalInSeconds = 3f;
        [SerializeField]
        private float _attackRange = 10f;
        [SerializeField]
        internal float _aimSpeed = 5f;

        internal ITargetable _target = null;
        internal Transform _targetTransform;
        internal Transform _transform;

        public bool Hidden => _hidden;
        public float DistanceToShowSelf => _distanceToShowSelf;
        public float AttackInterval => _attackIntervalInSeconds;
        public float AttackRange => _attackRange;
        public float AimSpeed => _aimSpeed;
        public GameObject Projectile => _projectile;

        public abstract void LaunchProjectile();

        public abstract GameObject GetTarget();
        private IEnumerator AttackCoroutine()
        {
            yield return new WaitForSeconds(_attackIntervalInSeconds);
            if (_target == null || Vector3.Distance(_transform.position, _targetTransform.position) > AttackRange)
            {
                var target = GetTarget();
                _target = target?.GetComponent<Targetable>();
                if (_target == null)
                    _target = target?.GetComponent<TargetableAttacker>();
                _targetTransform = target?.GetComponent<Transform>();
            }

            if(_target!=null)
                LaunchProjectile();

            StartCoroutine(AttackCoroutine());
        }
        private void Start()
        {
            _transform = GetComponent<Transform>();
            StartCoroutine(AttackCoroutine());
        }
        private void OnDrawGizmos()
        {
            UnityEditor.Handles.color = Color.red;
            UnityEditor.Handles.DrawWireDisc(GetComponent<Transform>().position - Vector3.down * -2, Vector3.up, _attackRange);
        }
    }
}
