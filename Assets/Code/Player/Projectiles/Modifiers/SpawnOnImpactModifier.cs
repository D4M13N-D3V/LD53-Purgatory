using Purgatory.Interfaces;
using UnityEngine;

namespace Purgatory.Player.Projectiles
{
	[CreateAssetMenu(menuName = "Purgatory/Projectile Modifiers/Spawn On Impact Modifier")]
	public class SpawnOnImpactModifier : ProjectileModifier
	{
		public override bool AppliesToChildren => appliesToChildren;
		[SerializeField] private bool appliesToChildren = true;

		[SerializeField] private GameObject prefab;
		
		protected override void OnProjectileRegistered(ProjectileBehavior projectile)
		{
			projectile.Impact += OnImpact;
		}

		protected override void OnProjectileRemoved(ProjectileBehavior projectile)
		{
			projectile.Impact -= OnImpact;
		}

		private void OnImpact(ITargetable enemy, RaycastHit hit)
		{
			Instantiate(prefab, hit.point, Quaternion.LookRotation(hit.normal, Vector3.up));
		}
	}
}