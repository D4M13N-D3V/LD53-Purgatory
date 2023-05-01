using System;
using System.Collections.Generic;
using Purgatory.Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Purgatory.Player.Projectiles
{
	public class ProjectileBehavior : MonoBehaviour
	{
		private const float HOMING_COOLDOWN = 0.25f;
		private const int COLLIDER_BUFFER_SIZE = 16;
		private const int ENEMY_BUFFER_SIZE = 8;

		public Action<ITargetable, RaycastHit> EnemyHit;
		public Action<ITargetable, RaycastHit> Impact;

		public Transform VisualTransform;
		public ProjectileHandler Handler { get; private set; }
		public bool IsChild = false;
		public ProjectileStats Stats = new();
		private ProjectileStats cachedStats;

		[SerializeField] private LayerMask enemyLayer;
		
		private int timesPierced = 0;
		private int timesBounced = 0;
		private Collider homingTarget;
		private float homingUpdateTimer;
		// TODO - Replace these with a central pool of some kind later
		private readonly Collider[] colliderBuffer = new Collider[COLLIDER_BUFFER_SIZE];
		private readonly RaycastHit[] hitBuffer = new RaycastHit[ENEMY_BUFFER_SIZE];
		
		public void Initialize(ProjectileHandler handler, Collider enemy)
		{
			ResetValues();
			Handler = handler;

			foreach (var mod in Handler.Modifiers)
			{
				mod.RegisterProjectile(this);
			}
			
			// Look at enemy
			var pos = transform.position;
			var enemyPos = enemy.transform.position;
			enemyPos.y = pos.y;
			var enemyVel = new Vector3(0, 0, 4.15f);	// Yes, this is a hack. It's a hack that works though.
			var predictedPos = enemyPos + enemyVel * (Vector3.Distance(pos, enemyPos) / Stats.Speed);

			var dir = (predictedPos - pos).normalized;
			var rot = Quaternion.LookRotation(dir, Vector3.up);
			transform.rotation = rot;
		}

		protected void ResetValues()
		{
			Stats = cachedStats;
			Handler = null;
			timesPierced = 0;
			timesBounced = 0;
			homingTarget = null;
			homingUpdateTimer = 0;
		}
		
		protected void Awake()
		{
			cachedStats = Stats;
		}

		protected void OnDisable()
		{
			if (Handler != null && Handler.Modifiers != null)
			{
				foreach (var mod in Handler.Modifiers)
				{
					mod.RemoveProjectile(this);
				}
			}
		}

		protected void Update()
		{
			// Movement Processing
			Vector3 before = transform.position;
			Vector3 after = ProcessMovement();

			// Hit Processing
			int hitCount = Physics.CapsuleCastNonAlloc(before, after, Stats.Size, (after - before).normalized, hitBuffer, Vector3.Distance(before, after), enemyLayer);
			for (int i = 0; i < hitCount; i++)
			{
				var hit = hitBuffer[i];
				hit.transform.TryGetComponent(out ITargetable enemyTarget);

				// Bounce handling for non-enemies
				if (enemyTarget == null)
				{
					if(timesBounced < Stats.BounceCount)
					{
						timesBounced++;
						transform.rotation = Quaternion.LookRotation(Vector3.Reflect(transform.forward, hit.normal));
						continue;
					}
					
					// If we can't bounce, impact and return.
					ProcessImpact(null, hit);
					return;
				}
				
				enemyTarget.Damage(Stats.Damage);
				EnemyHit?.Invoke(enemyTarget, hit);
				
				// Optionally pierce
				if (timesPierced < Stats.PierceCount)
				{
					timesPierced++;
					continue;
				}

				// If we made it here, assume we fully impacted the enemy.
				ProcessImpact(enemyTarget, hit);
			}
		}

		protected void ProcessImpact(ITargetable target, RaycastHit hit)
		{
			Impact?.Invoke(target, hit);
			Destroy(gameObject);
		}
		
		protected Vector3 ProcessMovement()
		{
			if (Stats.IsHoming)
			{
				homingUpdateTimer -= Time.deltaTime;

				// Check to see if we should re-look for an enemy in range
				if (homingUpdateTimer <= 0f)
				{
					homingUpdateTimer = HOMING_COOLDOWN;
					homingTarget = null;

					// Check for nearby enemies
					int hitCount = Physics.OverlapSphereNonAlloc(transform.position, Stats.HomingRange, colliderBuffer, enemyLayer);
					float leastDist = float.MaxValue;
					for (int i = 0; i < hitCount; i++)
					{
						var col = colliderBuffer[i];
						float dist = Vector3.Distance(transform.position, col.transform.position);
						if (dist < leastDist)
						{
							leastDist = dist;
							homingTarget = col;
						}
					}
				}

				if (homingTarget)
				{
					transform.LookAt(homingTarget.transform.position);
				}
			}

			transform.position += transform.forward * (Stats.Speed * Time.deltaTime);
			return transform.position;
		}
	}
}