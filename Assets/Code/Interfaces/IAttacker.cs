using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Purgatory.Interfaces
{
    public interface IAttacker
    {
        bool Hidden { get; }
        float DistanceToShowSelf { get; }
        float AttackInterval { get;  }
        float AttackRange { get; }
        float AimSpeed { get; }
        GameObject Target { get; }
        GameObject Projectile { get; }
        void LaunchProjectile();
        GameObject GetTarget();
    }
}
