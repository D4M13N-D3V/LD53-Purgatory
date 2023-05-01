using Purgatory.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Purgatory.Impl
{
    public abstract class Targetable : MonoBehaviour, ITargetable
    {
        [SerializeField]
        internal int _currentHealth = 1;
        [SerializeField]
        internal int _maximumHealth = 1;
        internal int _minimumHealth = 0;

        public int CurrentHP => _currentHealth;
        public bool Alive { get => _currentHealth <= _minimumHealth; }

        public delegate void OnDamaged(int amount);
        public OnDamaged onDamaged;
            
        public delegate void OnHealed(int amount);
        public OnHealed onHealed;

        public delegate void OnDeath();
        public OnDeath onDeath;


        public void SetMaximumHealth(int amount)
        {
            
            _maximumHealth = amount;
        }

        public int GetMaximumHealth()
        {
            return _maximumHealth;
        }

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

        public abstract void DeathLogic();

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
