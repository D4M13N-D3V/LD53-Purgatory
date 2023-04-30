using Purgatory.Impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Purgatory.Interfaces;

namespace Purgatory.Player
{
    public class LanternController : Attacker
    {
        private ITargetable _target = null;
        private Transform _targetTransform;


        // Update is called once per frame
        void Update()
        {

        }


        public override void LaunchProjectile()
        {
            Debug.Log("Projectile launched!");
        }
    }
}