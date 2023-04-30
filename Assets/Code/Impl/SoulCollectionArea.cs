using Purgatory;
using Purgatory.Impl;
using Purgatory.Player;
using UnityEngine;

namespace Code.Impl
{
	public class SoulCollectionArea : MonoBehaviour
	{
		[SerializeField] private Soul[] souls;
		[SerializeField] private ParticleSystem particleSystem;
		
		private void OnTriggerEnter(Collider other)
		{
			var collector = other.GetComponent<SoulCollectionController>();
			if (collector)
			{
				foreach (var soul in souls)
				{
					soul.BeginCollect(collector);
				}
				
				particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
			}
		}
	}
}