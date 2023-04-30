﻿using System.Collections;
using UnityEngine;

namespace Assets.Code
{
    public abstract class TargetableAttacker : MonoBehaviour, ITargetable, IAttacker
    {
        [SerializeField]
        private GameObject _projectile = null;
        [SerializeField]
        private float _attackIntervalInSeconds = 3f;
        [SerializeField]
        private int _attackDamage = 1;
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

        public int CurrentHP { get => _currentHealth; }
        public bool Alive { get => _currentHealth <= _minimumHealth; }
        public bool Hidden => _hidden;
        public float DistanceToShowSelf => _distanceToShowSelf;

        public float AttackInterval => _attackIntervalInSeconds;

        public int AttackDamage => _attackDamage;


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

        public abstract void Attack();
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