using System;
using System.Collections.Generic;
using Purgatory.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace Purgatory.Player.Projectiles
{
	public class ProjectileHandler : MonoBehaviour
	{
		private const int CHECK_FRAMES = 4;
		
		public List<ProjectileModifier> Modifiers;
		
		[SerializeField] private ProjectileBehavior projectilePrefab;
		[SerializeField] private LayerMask enemyLayerMask;
		[SerializeField] private Vector3 checkCenter = new(15f, 15f, 15f);
		[SerializeField] private Vector3 checkExtents = new(30f, 30f, 30f);
		[SerializeField] private float attackCooldown = 5f;
		[SerializeField] private Vector3 projectileSpawnOffset = Vector3.zero;

		private float attackTimer;
		private int colliderCheckFrame;
		private int activeColliderCount;
		private Collider[] colliderBuffer = new Collider[128];	// Big ol buffer since we're checking the whole view
		private ColliderDistanceComparer colliderComparer;

		private void Start()
		{
			colliderComparer = new ColliderDistanceComparer(transform);
			// Unity fix so that playmode doesn't wreck shit
			for (int i = 0; i < Modifiers.Count; i++)
			{
				Modifiers[i] = Instantiate(Modifiers[i]);
			}
		}
		
		private void Update()
		{
			// Check for new enemies every N frames. Probably a micro-optimization but whatever.
			colliderCheckFrame--;
			if (colliderCheckFrame <= 0)
			{
				colliderCheckFrame = CHECK_FRAMES;
				CheckForEnemies();
			}
			
			// Attacking processing
			attackTimer -= Time.deltaTime;
			if (activeColliderCount > 0 && attackTimer <= 0f)
			{
				attackTimer = attackCooldown;
				for (int i = 0; i < activeColliderCount; i++)
				{
					if (!colliderBuffer[i].TryGetComponent(out ITargetable enemy))
					{
						continue;
					}
					
					// Ignore player
					if (enemy is PlayerController)
						continue;

					ShootAtEnemy(colliderBuffer[i]);
					break;
				}
			}
			
			foreach (var modifier in Modifiers)
			{
				modifier.Update();
			}
		}

		private void CheckForEnemies()
		{
			activeColliderCount = Physics.OverlapBoxNonAlloc(checkCenter, checkExtents, colliderBuffer, Quaternion.identity, enemyLayerMask);
			if (activeColliderCount <= 0)
				return;
			
			// Sort them by closest to 0(aka, the player)
			Array.Sort(colliderBuffer, 0, activeColliderCount, colliderComparer);
		}

		private void ShootAtEnemy(Collider col)
		{
			// Instantiate a new projectile, facing collider/enemy
			Vector3 spawnPos = transform.position + projectileSpawnOffset;
			Quaternion spawnRot = Quaternion.LookRotation(col.transform.position - transform.position, Vector3.up);
			var projectileInstance = Instantiate(projectilePrefab, spawnPos, spawnRot);
			projectileInstance.Initialize(this, col);
		}
		
		private class ColliderDistanceComparer : IComparer<Collider>
		{
			private Transform tform;
			public ColliderDistanceComparer(Transform tform)
			{
				this.tform = tform;
			}
			public int Compare(Collider a, Collider b)
			{
				if (a == null)
					return -1;
				if (b == null)
					return 1;

				float distA = Vector3.Distance(a.transform.position, tform.position);
				float distB = Vector3.Distance(b.transform.position, tform.position);
				return distA.CompareTo(distB);
			}
		}

		#if UNITY_EDITOR
		private void OnDrawGizmosSelected()
		{
			Gizmos.DrawWireCube(checkCenter, checkExtents * 2);
		}
		#endif
	}
}