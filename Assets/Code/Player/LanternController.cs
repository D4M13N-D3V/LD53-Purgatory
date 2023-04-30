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
            if (_targetTransform != null)
            {
                var rotation = Quaternion.LookRotation(_targetTransform.position - transform.position);
                var newProject = GameObject.Instantiate(Projectile, transform.position, rotation);
                newProject.transform.rotation = rotation;
                var projectile = newProject.GetComponent<Projectile>();
                projectile.TargetLocation = _targetTransform.position;
                Debug.Log("Player projectile launched!");
            }
        }

        public override GameObject GetTarget()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, AttackRange);
            var target = hitColliders.Where(x=> x.GetComponent<EnemyController>()!=null).OrderBy(x => Guid.NewGuid()).FirstOrDefault();

            if (target != null)
                Debug.Log($"Lantern targeting gameobject {target.gameObject.name}");

            return target?.gameObject;
        }
    }
}