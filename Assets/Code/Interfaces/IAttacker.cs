using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttacker
{
    bool Hidden { get; }
    float DistanceToShowSelf { get; }
    float AttackInterval { get; }
    float AttackRange { get; }
    int AttackDamage { get; }
    GameObject Projectile { get; }
    void Attack();
}
