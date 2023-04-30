using System.Collections;
using UnityEngine;

namespace Assets.Code
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
        private int _attackDamage = 1;
        [SerializeField]
        private float _attackRange = 10;

        public bool Hidden => _hidden;
        public float DistanceToShowSelf => _distanceToShowSelf;

        public float AttackInterval => _attackIntervalInSeconds;

        public int AttackDamage => _attackDamage;

        public float AttackRange => _attackRange;

        public GameObject Projectile => _projectile;

        public abstract void Attack();
        private void OnDrawGizmos()
        {
            UnityEditor.Handles.color = Color.red;
            UnityEditor.Handles.DrawWireDisc(GetComponent<Transform>().position - Vector3.down * -2, Vector3.up, _attackRange);
        }
    }
}