using Purgatory.Interfaces;
using Purgatory.Player;
using System.Collections;
using UnityEngine;

namespace Purgatory.Impl
{
    public abstract class Soul : MonoBehaviour, ISoul
    {
        [SerializeField]
        private int _amount = 1;

        public int Amount => _amount;

		[SerializeField]
		protected float moveSpeed = 5f;
		[SerializeField]
		protected float rotationSpeed = 5f;
		[SerializeField]
		protected float collectionDistance = 0.2f;

		[SerializeField]
		protected GameObject collectionVfx;
		
		protected bool IsBeingCollected = false;
		protected SoulCollectionController Collector = null;

		public virtual void CleanupAfterCollection()
		{
			Destroy(gameObject);
		}

		public void BeginCollect(SoulCollectionController collector)
		{
			if (IsBeingCollected)
				return;
			
			IsBeingCollected = true;
			Collector = collector;
		}

		protected void Update()
		{
			if (IsBeingCollected && Collector)
			{
				var targetPosition = Collector.transform.position;
				targetPosition.y = transform.position.y;	// We ignore the Y plane since gameplay is primarily on XZ
				transform.LookAt(targetPosition);
				transform.position += transform.forward * (moveSpeed * Time.deltaTime);
				
				var dist = Vector3.Distance(transform.position, targetPosition);
				if (dist < collectionDistance)
				{
					Collect(Collector);
					CleanupAfterCollection();
				}
			}
		}
		
		public void Collect(SoulCollectionController soulCollectionController)
        {
            soulCollectionController.AddSoul(Amount);

			if (collectionVfx)
			{
				Instantiate(collectionVfx, transform.position, Quaternion.identity);
			}
        }
	}
}