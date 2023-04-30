using Purgatory.Impl;
using Purgatory.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Purgatory.Enemy
{
    public class EnemyController : TargetableAttacker
    {

        public EnemyController() : base()
        {
        }

        public override void DeathLogic()
        {
            Destroy(this.gameObject);
        }

        public override GameObject GetTarget()
        {
            Collider[] hitColliders = Physics.OverlapSphere(_transform.position, AttackRange);
            var target = hitColliders.Where(x => x.GetComponent<PlayerController>() != null).OrderBy(x => Guid.NewGuid()).FirstOrDefault();
            Debug.Log($"Enemy {this.gameObject.name} targeting player");
            return target?.gameObject;
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
