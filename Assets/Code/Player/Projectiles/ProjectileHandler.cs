using System;
using System.Collections.Generic;
using System.Linq;
using Purgatory.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Purgatory.Player.Projectiles
{
	public class ProjectileHandler : MonoBehaviour
	{
		private const int CHECK_FRAMES = 4;
		
		public List<ProjectileModifier> Modifiers;
		
		[SerializeField] private GameObject projectilePrefab;
		[SerializeField] private LayerMask enemyLayerMask;
		[SerializeField] private Vector3 checkCenter = new(15f, 15f, 15f);
		[SerializeField] private Vector3 checkExtents = new(30f, 30f, 30f);
		[SerializeField] public float attackCooldown = 5f;
		[SerializeField] public float attackRangeModifier = 2f;
		[SerializeField] private Vector3 projectileSpawnOffset = Vector3.zero;
		[SerializeField] private InputActionAsset actions;
		[SerializeField] public List<GameObject> AvailableProjectiles = new List<GameObject>();
		[SerializeField] public GameObject _currentProjectile;
		
		private int _currentProjectileIndex = 0;
		private InputAction _moveAction;
		private float attackTimer;
		private int colliderCheckFrame;
		private int activeColliderCount;
		private Collider[] colliderBuffer = new Collider[128];	// Big ol buffer since we're checking the whole view
		private ColliderDistanceComparer colliderComparer;

		public static ProjectileHandler instance;
		public ProjectileHandler()
		{
			instance = this;
		}
		private void Start()
		{
			AvailableProjectiles = GameManager.instance.AvailableProjectiles;
			_moveAction = actions.FindActionMap("gameplay").FindAction("move", true);
			actions.FindActionMap("gameplay").FindAction("previous_projectile").performed += PreviousProjectile;
			actions.FindActionMap("gameplay").FindAction("next_projectile").performed += NextProjectile;
			colliderComparer = new ColliderDistanceComparer(transform);
			// Unity fix so that playmode doesn't wreck shit
			for (int i = 0; i < Modifiers.Count; i++)
			{
				Modifiers[i] = Instantiate(Modifiers[i]);
			}
		}
		
		private void Update()
		{
			if (_currentProjectile == null)
			{
				_currentProjectile = AvailableProjectiles.FirstOrDefault();
				_currentProjectileIndex = 0;
			}


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

		private void NextProjectile(InputAction.CallbackContext obj)
		{
			if (_currentProjectileIndex == AvailableProjectiles.Count() - 1)
			{
				var first = AvailableProjectiles.First();
				_currentProjectileIndex = 0;
				_currentProjectile = first;
			}
			else
			{
				_currentProjectileIndex++;
				_currentProjectile = AvailableProjectiles[_currentProjectileIndex];
			}
		}

		private void PreviousProjectile(InputAction.CallbackContext obj)
		{
			if (_currentProjectileIndex == 0)
			{
				var last = AvailableProjectiles.Last();
				_currentProjectileIndex = AvailableProjectiles.IndexOf(last);
				_currentProjectile = last;
			}
			else
			{
				_currentProjectileIndex--;
				_currentProjectile = AvailableProjectiles[_currentProjectileIndex];
			}
		}

		private void CheckForEnemies()
		{
			activeColliderCount = Physics.OverlapBoxNonAlloc(checkCenter, checkExtents * attackRangeModifier, colliderBuffer, Quaternion.identity, enemyLayerMask);
			if (activeColliderCount <= 0)
				return;
			
			// Sort them by closest to 0(aka, the player)
			Array.Sort(colliderBuffer, 0, activeColliderCount, colliderComparer);
		}

		private void ShootAtEnemy(Collider col)
		{
			if (_currentProjectile == null)
				return;
			// Instantiate a new projectile, facing collider/enemy
			Vector3 spawnPos = transform.position + projectileSpawnOffset;
			Quaternion spawnRot = Quaternion.LookRotation(col.transform.position - transform.position, Vector3.up);
			var projectileInstance = Instantiate(_currentProjectile, spawnPos, spawnRot);
			projectileInstance.GetComponent<ProjectileBehavior>().Initialize(this, col);
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
			Gizmos.DrawWireCube(checkCenter, checkExtents * attackRangeModifier * 2);
		}
		#endif
	}
}