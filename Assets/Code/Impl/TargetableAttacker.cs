using Purgatory.Interfaces;
using System.Collections;
using UnityEngine;

namespace Purgatory.Impl
{
    public abstract class TargetableAttacker : MonoBehaviour, ITargetable, IAttacker
    {
        [SerializeField]
        private GameObject _projectile = null;
        [SerializeField]
        private float _attackIntervalInSeconds = 3f;
        [SerializeField]
        private int _currentHealth;
        [SerializeField]
        private bool _hidden = false;
        [SerializeField]
        private float _distanceToShowSelf = 10f;
        [SerializeField]
        private int _maximumHealth = 100;
        [SerializeField]
        private int _minimumHealth = 0;
        [SerializeField]
        private float _attackRange = 10;
        [SerializeField]
        internal float _aimSpeed = 5f;


        internal ITargetable _target = null;
        internal Transform _targetTransform;
        internal Transform _transform;

        public int CurrentHP { get => _currentHealth; }
        public bool Alive { get => _currentHealth <= _minimumHealth; }
        public bool Hidden => _hidden;
        public float AimSpeed => _aimSpeed;
        public float DistanceToShowSelf => _distanceToShowSelf;
        public float AttackInterval => _attackIntervalInSeconds;
        public float AttackRange => _attackRange;
        public GameObject Projectile => _projectile;

        delegate void OnDamaged(int amount);
        OnDamaged onDamaged;

        delegate void OnHealed(int amount);
        OnHealed onHealed;

        delegate void OnDeath();
        OnDeath onDeath;

        public TargetableAttacker() : base()
        {
            _currentHealth = _maximumHealth;
        }

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

            if (_target != null)
                LaunchProjectile();

            StartCoroutine(AttackCoroutine());
        }
        void Start()
        {
            _transform = GetComponent<Transform>();
            StartCoroutine(AttackCoroutine());
        }

        public abstract GameObject GetTarget();
        public abstract void LaunchProjectile();
        public abstract void DeathLogic();

        public void Damage(int amount)
        {
            if (amount > 0)
            {
                onDamaged?.Invoke(amount);
                Debug.Log($"Applying {amount} of damage.");
                if (_currentHealth > amount)
                {
                    _currentHealth -= amount;
                }
                else
                {
                    _currentHealth = _minimumHealth;
                    Die();
                }
            }
            else
            {
                Debug.LogWarning("Tried to apply a negative amount of damage.");
            }
        }

        public void Die()
        {
            Debug.Log("Health went under minimum health, entity died.");
            onDeath?.Invoke();
            DeathLogic();
        }

        public void Heal(int amount)
        {
            if (amount > 0)
            {
                onHealed?.Invoke(amount);
                Debug.Log($"Applying {amount} of healing.");
                _currentHealth += amount;
                if (_currentHealth > _maximumHealth)
                    _currentHealth = _maximumHealth;
            }
            else
            {
                Debug.LogWarning("Tried to apply a negative amount of healing.");
            }
        }
    }
}