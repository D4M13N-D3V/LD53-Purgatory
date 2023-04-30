using Purgatory.Impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Purgatory.Enemy
{
    public class BaseEnemyController : TargetableAttacker
    {

        public BaseEnemyController() : base()
        {
        }

        public override void DeathLogic()
        {
            Destroy(this.gameObject);
        }

        public override void LaunchProjectile()
        {
            Debug.Log("Enemy projectile launched!");
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}
