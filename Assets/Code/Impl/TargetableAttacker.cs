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
        private int _currentHealth;
        [SerializeField]
        private bool _hidden = false;
        [SerializeField]
        private int _maximumHealth = 100;
        [SerializeField]
        private int _minimumHealth = 0;

        public float DistanceToShowSelf { get; set; }
        public GameObject Projectile { get; set; }
        public float AttackInterval { get; set; }
        public float AttackRange { get; set; }
        public float AimSpeed { get; set; }

        internal ITargetable _target = null;
        [SerializeField]
        internal Transform _targetTransform;

        public int CurrentHP { get => _currentHealth; }
        public bool Alive { get => _currentHealth <= _minimumHealth; }
        public bool Hidden => _hidden;
        public GameObject Target => _targetTransform.gameObject;

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

        private void OnDrawGizmos()
        {
            UnityEditor.Handles.color = Color.magenta;
            UnityEditor.Handles.DrawWireDisc(GetComponent<Transform>().position - Vector3.down * -2, Vector3.up, AttackRange);
        }
        private IEnumerator AttackCoroutine()
        {
            if (_target == null || _targetTransform == null || _targetTransform != null && Vector3.Distance(transform.position, _targetTransform.position) > AttackRange)
            {
                var target = GetTarget();
                _target = target?.GetComponent<Targetable>();
                if (_target == null)
                    _target = target?.GetComponent<TargetableAttacker>();
                _targetTransform = target?.GetComponent<Transform>();
            }

            if (_target != null)
                LaunchProjectile();

            yield return new WaitForSeconds(AttackInterval);

            StartCoroutine(AttackCoroutine());
        }
        void Start()
        {
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