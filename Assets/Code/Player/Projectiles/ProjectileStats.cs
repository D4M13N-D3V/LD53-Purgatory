using System;

namespace Purgatory.Player.Projectiles
{
	[Serializable]
	public struct ProjectileStats
	{
		public int Damage;				// Amount of damage this projectile will do to an enemy
		public bool IsHoming;			// Automatically moves towards targets
		public int PierceCount;			// Amount of times to pierce(skip through) enemies
		public int BounceCount;			// Amount of times to bounce off of the level boundaries
		public float Speed;				// Movement speed for this projectile
		public float HomingRange;		// The distance this projectile will check for nearby enemies
		public float HomingStrength;	// The strength at which this projectile will move towards the enemy
		public float Size;				// The size of this projectile, for physics checks

		public ProjectileStats(ProjectileStats other)
		{
			Damage = other.Damage;
			IsHoming = other.IsHoming;
			PierceCount = other.PierceCount;
			BounceCount = other.BounceCount;
			Speed = other.Speed;
			HomingRange = other.HomingRange;
			HomingStrength = other.HomingStrength;
			Size = other.Size;
		}

		public ProjectileStats ReduceForChild()
		{
			return new ProjectileStats()
			{
				Damage = Damage / 2,
				IsHoming = IsHoming,
				PierceCount = 0,
				BounceCount = BounceCount / 2,
				Speed = Speed,
				HomingRange = HomingRange,
				HomingStrength = HomingStrength / 2f,
				Size = Size / 2f
			};
		}
	}
}