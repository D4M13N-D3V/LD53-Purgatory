using Purgatory.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace Purgatory.Player.Projectiles
{
	[CreateAssetMenu(menuName = "Purgatory/Projectile Modifiers/AoE DoT")]
	public class AoEDoTModifier : ProjectileModifier
	{
		public override bool AppliesToChildren => true;
		public float Radius = 3f;
		public int Damage = 2;
		public float AttackRate = 1f;
		
		[SerializeField] private float childRadiusDivider = 2f;
		[SerializeField] private int childDamageDivider = 2;
		
		[SerializeField] private GameObject prefab;

		private readonly Collider[] hitBuffer = new Collider[32];
		private float attackTimer = 0f;

		private float GetRadius(bool isChild) => isChild ? Radius / childRadiusDivider : Radius;
		private int GetDamage(bool isChild) => isChild ? Damage / childDamageDivider : Damage;
		
		protected override void OnProjectileRegistered(ProjectileBehavior projectile)
		{
			var instance = Instantiate(prefab);
			instance.transform.position = projectile.transform.position;
			instance.transform.parent = projectile.ExtraVisuals;
			instance.transform.localScale = Vector3.one * GetRadius(projectile.IsChild);
		}

		public override void Update()
		{
			attackTimer -= Time.deltaTime;
			if (attackTimer <= 0f)
			{
				attackTimer = AttackRate;
				foreach (var projectile in Projectiles)
				{
					int hitCount = Physics.OverlapSphereNonAlloc(projectile.transform.position, GetRadius(projectile.IsChild), hitBuffer, 1 << 6);
					for (int i = 0; i < hitCount; i++)
					{
						if (!hitBuffer[i].TryGetComponent(out ITargetable target))
							continue;
						target.Damage(GetDamage(projectile.IsChild));
					}
				}
			}
		}
	}
}