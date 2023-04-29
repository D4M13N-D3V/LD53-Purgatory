using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Targetable : MonoBehaviour, ITargetable
{
    private int _currentHealth;

    public int MaximumHealth = 1;
    public int MinimumHealth = 0;

    public int CurrentHP { get => _currentHealth; }
    public bool Alive { get => _currentHealth <= MinimumHealth; }

    delegate void OnDamaged(int amount);
    OnDamaged onDamaged;

    delegate void OnHealed(int amount);
    OnHealed onHealed;

    delegate void OnDeath();
    OnDeath onDeath;


    void Start()
    {
        _currentHealth = MaximumHealth;
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
                _currentHealth = MinimumHealth;
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
        onDeath?.Invoke();
    }

    public abstract void DeathLogic();

    public void Heal(int amount)
    {
        if (amount > 0)
        {
            onHealed?.Invoke(amount);
            Debug.Log($"Applying {amount} of healing.");
            _currentHealth += amount;
            if (_currentHealth > MaximumHealth)
                _currentHealth = MaximumHealth;
        }
        else
        {
            Debug.LogWarning("Tried to apply a negative amount of healing.");
        }
    }
}
