using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Purgatory.Interfaces
{
    public interface IAttacker
    {
        bool Hidden { get; }
        float DistanceToShowSelf { get; set; }
        float AttackInterval { get; set;  }
        float AttackRange { get; set; }
        float AimSpeed { get; set; }
        GameObject Target { get; }
        GameObject Projectile { get; set; }
        void LaunchProjectile();
        GameObject GetTarget();
    }
}
