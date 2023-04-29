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

        public bool Hidden => _hidden;
        public float DistanceToShowSelf => _distanceToShowSelf;

        public float AttackInterval => _attackIntervalInSeconds;

        public int AttackDamage => _attackDamage;

        public GameObject Projectile => _projectile;

        public abstract void Attack();
    }
}