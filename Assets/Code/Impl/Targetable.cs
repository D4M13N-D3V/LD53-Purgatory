using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Targetable : MonoBehaviour, ITargetable
{
    [SerializeField]
    private int _currentHealth;
    [SerializeField]
    private int _maximumHealth = 1;
    [SerializeField]
    private int _minimumHealth = 0;
 
    public int CurrentHP { get => _currentHealth; }
    public bool Alive { get => _currentHealth <= _minimumHealth; }

    delegate void OnDamaged(int amount);
    OnDamaged onDamaged;

    delegate void OnHealed(int amount);
    OnHealed onHealed;

    delegate void OnDeath();
    OnDeath onDeath;

    public Targetable() : base()
    {
        _currentHealth = _maximumHealth;
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
