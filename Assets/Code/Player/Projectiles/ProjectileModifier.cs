using System.Collections.Generic;
using UnityEngine;

namespace Purgatory.Player.Projectiles
{
	public abstract class ProjectileModifier : ScriptableObject
	{
		protected readonly List<ProjectileBehavior> Projectiles = new();

		public abstract bool AppliesToChildren { get; }
		
		public void RegisterProjectile(ProjectileBehavior projectile)
		{
			OnProjectileRegistered(projectile);
			Projectiles.Add(projectile);
		}

		public void RemoveProjectile(ProjectileBehavior projectile)
		{
			if (!Projectiles.Contains(projectile))
				return;
			
			OnProjectileRemoved(projectile);
			Projectiles.Remove(projectile);
		}

		protected virtual void OnProjectileRegistered(ProjectileBehavior projectile) { }
		protected virtual void OnProjectileRemoved(ProjectileBehavior projectile) { }
		public virtual void Update() {}
		
	}
}