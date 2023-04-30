using Purgatory.Impl;
using Purgatory.Interfaces;
using System.Collections;
using UnityEngine;

namespace Assets.Code.Enemy
{
    public class EnemySoul : Soul
    {
        public override void CleanupAfterCollection()
        {
            Debug.Log("Enemy soul has been collected.");
            Destroy(gameObject);
        }
	}
}