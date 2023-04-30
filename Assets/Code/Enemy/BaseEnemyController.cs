using Purgatory.Impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Purgatory.Enemy
{
    public class BaseEnemyController : Targetable
    {

        public BaseEnemyController() : base()
        {
        }

        public override void DeathLogic()
        {
            Destroy(this.gameObject);
        }

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
