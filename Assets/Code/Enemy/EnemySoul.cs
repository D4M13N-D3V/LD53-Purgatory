using Purgatory.Impl;
using Purgatory.Interfaces;
using System.Collections;
using UnityEngine;

namespace Assets.Code.Enemy
{
    public class EnemySoul : Soul
    {
        public override void ExtraUpdateLogic()
        {
            //TODO: check distance to player and if we are close enough start collecting


            if (IsBeingCollected)
                return;

            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.02f);
            base.ExtraUpdateLogic();
        }
        public override void CleanupAfterCollection()
        {
            Debug.Log("Enemy soul has been collected.");
            Destroy(gameObject);
        }
	}
}