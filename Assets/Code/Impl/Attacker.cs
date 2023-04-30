using Purgatory.Interfaces;
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

        public bool Hidden => _hidden;
        public float DistanceToShowSelf => _distanceToShowSelf;
        public float AttackInterval => _attackIntervalInSeconds;
        public float AttackRange => _attackRange;
        public float AimSpeed => _aimSpeed;
        public GameObject Projectile => _projectile;
        public GameObject Target => _targetTransform.gameObject;
        public abstract void LaunchProjectile();

        public abstract GameObject GetTarget();
        private IEnumerator AttackCoroutine()
        {
            if (_target == null  || _targetTransform==null || _targetTransform!=null && Vector3.Distance(transform.position, _targetTransform.position) > AttackRange)
            {
                var target = GetTarget();
                _target = target?.GetComponent<Targetable>();
                if (_target == null)
                    _target = target?.GetComponent<TargetableAttacker>();
                _targetTransform = target?.GetComponent<Transform>();
            }

            if(_target!=null)
                LaunchProjectile();

            yield return new WaitForSeconds(_attackIntervalInSeconds);

            StartCoroutine(AttackCoroutine());
        }
        private void Start()
        {
                StartCoroutine(AttackCoroutine());

        }
        private void OnDrawGizmos()
        {
            UnityEditor.Handles.color = Color.red;
            UnityEditor.Handles.DrawWireDisc(GetComponent<Transform>().position - Vector3.down * -2, Vector3.up, _attackRange);
        }
    }
}
