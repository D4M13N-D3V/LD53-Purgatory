using Purgatory.Impl;
using Purgatory.Interfaces;
using System.Collections;
using UnityEngine;

namespace Assets.Code.Enemy
{
    public class EnemySoul : Soul
    {
        public override void Collected()
        {
            Debug.Log("Enemy soul has been collected.");
            Destroy(gameObject);
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}