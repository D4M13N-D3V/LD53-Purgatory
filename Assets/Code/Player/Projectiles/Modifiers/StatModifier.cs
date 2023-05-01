using UnityEngine;
using UnityEngine.Serialization;

namespace Purgatory.Player.Projectiles
{
	[CreateAssetMenu(menuName = "Purgatory/Projectile Modifiers/Stat Modifier")]
	public class StatModifier : ProjectileModifier
	{
		public override bool AppliesToChildren => false;
		
		[FormerlySerializedAs("MathModifier")] [FormerlySerializedAs("MathMode")] public Modifier Mod = Modifier.Additive;
		public Stat TargetStat = Stat.Damage;

		public float Value;

		protected override void OnProjectileRegistered(ProjectileBehavior projectile)
		{
			switch (TargetStat)
			{
				case Stat.Damage:
					ApplyTo(ref projectile.Stats.Damage);
					break;
				case Stat.PierceCount:
					ApplyTo(ref projectile.Stats.PierceCount);
					break;
				case Stat.BounceCount:
					ApplyTo(ref projectile.Stats.BounceCount);
					break;
				case Stat.Speed:
					ApplyTo(ref projectile.Stats.Speed);
					break;
				case Stat.HomingRange:
					ApplyTo(ref projectile.Stats.HomingRange);
					break;
				case Stat.HomingStrength:
					ApplyTo(ref projectile.Stats.HomingStrength);
					break;
				case Stat.Size:
					ApplyTo(ref projectile.Stats.Size);
					break;
			}
		}

		private void ApplyTo(ref int target)
		{
			switch (Mod)
			{
				case Modifier.Additive:
					target += (int)Value;
					break;
				case Modifier.Direct:
					target = (int)Value;
					break;
				case Modifier.Multiplicative:
					target *= (int)Value;
					break;
			}
		}

		private void ApplyTo(ref float target)
		{
			switch (Mod)
			{
				case Modifier.Additive:
					target += Value;
					break;
				case Modifier.Direct:
					target = Value;
					break;
				case Modifier.Multiplicative:
					target *= Value;
					break;
			}
		}
		
		public enum Stat
		{
			Damage, PierceCount, BounceCount, Speed, HomingRange, HomingStrength, Size
		}

		public enum Modifier
		{
			Additive, Multiplicative, Direct
		}
	}
}