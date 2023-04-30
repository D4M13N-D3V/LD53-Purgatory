using Purgatory.Impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Purgatory.Interfaces;
using Purgatory.Enemy;

namespace Purgatory.Player
{
    public class LanternController : Attacker
    {
        // Update is called once per frame
        void Update()
        {

        }


        public override void LaunchProjectile()
        {
            Debug.Log("Projectile launched!");
        }

        public override GameObject GetTarget()
        {
            Collider[] hitColliders = Physics.OverlapSphere(_transform.position, AttackRange);
            var target = hitColliders.Where(x=> x.GetComponent<EnemyController>()!=null).OrderBy(x => Guid.NewGuid()).FirstOrDefault();

            if (target != null)
                Debug.Log($"Lantern targeting gameobject {target.gameObject.name}");

            return target?.gameObject;
        }
    }
}