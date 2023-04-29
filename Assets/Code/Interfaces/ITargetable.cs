using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetable
{
    int CurrentHP { get; }
    bool Alive { get; }
    void Die();
    void Heal(int amount);
    void Damage(int amount);
}
