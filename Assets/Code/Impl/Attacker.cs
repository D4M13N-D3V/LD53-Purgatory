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
        private float _attackRange = 10;

        private ITargetable _target = null;
        private Transform _targetTransform;
        internal Transform _transform;

        public bool Hidden => _hidden;
        public float DistanceToShowSelf => _distanceToShowSelf;
        public float AttackInterval => _attackIntervalInSeconds;
        public float AttackRange => _attackRange;
        public GameObject Projectile => _projectile;

        public abstract void LaunchProjectile();

        private IEnumerator AttackCoroutine()
        {
            yield return new WaitForSeconds(_attackIntervalInSeconds);
            if (_target == null || Vector3.Distance(_transform.position, _targetTransform.position) > AttackRange)
            {
                Collider[] hitColliders = Physics.OverlapSphere(_transform.position, AttackRange);
                var target = hitColliders.OrderBy(x => Guid.NewGuid()).FirstOrDefault();
                _target = target.GetComponent<Targetable>();
                if (_target == null)
                    _target = target.GetComponent<TargetableAttacker>();
                _targetTransform = target.GetComponent<Transform>();
            }
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
