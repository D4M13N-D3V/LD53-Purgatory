using Purgatory.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace Purgatory.Player.Projectiles
{
	[CreateAssetMenu(menuName = "Purgatory/Projectile Modifiers/Spawn On Hit Modifier")]
	public class SpawnOnHitModifier : ProjectileModifier
	{
		public override bool AppliesToChildren => appliesToChildren;
		[SerializeField] private bool appliesToChildren = true;
		[SerializeField] private GameObject prefab;
		
		protected override void OnProjectileRegistered(ProjectileBehavior projectile)
		{
			projectile.EnemyHit += OnEnemyHit;
		}

		private void OnEnemyHit(ITargetable enemy, RaycastHit hit)
		{
			Instantiate(prefab, hit.point, Quaternion.LookRotation(hit.normal, Vector3.up));
		}

		protected override void OnProjectileRemoved(ProjectileBehavior projectile)
		{
			projectile.EnemyHit -= OnEnemyHit;
		}
	}
}