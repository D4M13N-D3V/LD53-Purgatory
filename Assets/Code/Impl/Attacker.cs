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
        public float DistanceToShowSelf { get; set; }
        public GameObject Projectile { get; set; }
        public float AttackInterval { get; set; }
        public float AttackRange { get; set; }
        public float AimSpeed { get; set; }

        internal ITargetable _target = null;
        internal Transform _targetTransform;

        public bool Hidden => _hidden;
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

            yield return new WaitForSeconds(AttackInterval);

            StartCoroutine(AttackCoroutine());
        }
        private void Start()
        {
                StartCoroutine(AttackCoroutine());

        }
        private void OnDrawGizmos()
        {
            UnityEditor.Handles.color = Color.red;
            UnityEditor.Handles.DrawWireDisc(GetComponent<Transform>().position - Vector3.down * -2, Vector3.up, AttackRange);
        }
    }
}
