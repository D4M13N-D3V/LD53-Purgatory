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
        [SerializeField]
        public GameObject _enemySoul;

        public EnemyController() : base()
        {
        }

        public override void DeathLogic()
        {
            Destroy(this.gameObject);
        }

        public override GameObject GetTarget()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, AttackRange);
            var target = hitColliders.Where(x => x.GetComponent<PlayerController>() != null).OrderBy(x => Guid.NewGuid()).FirstOrDefault();
            
            if(target!=null)
                Debug.Log($"Enemy {this.gameObject.name} targeting player");

            return target?.gameObject;
        }

        public override void LaunchProjectile()
        {
            var rotation = Quaternion.LookRotation(_targetTransform.position - transform.position);
            var newProject = GameObject.Instantiate(Projectile, transform.position+(transform.forward*1.5f), rotation);
            Debug.Log("Enemy projectile launched!");
        }
        // Update is called once per frame
        void Update()
        {
            Quaternion _lookRotation = Quaternion.LookRotation((_targetTransform.position - transform.position).normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * _aimSpeed);
        }
    }
}
