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
		
		public ProjectileModifier CurrentModifier;
		
		[SerializeField] private GameObject projectilePrefab;
		[SerializeField] private LayerMask enemyLayerMask;
		[SerializeField] private Vector3 checkCenter = new(15f, 15f, 15f);
		[SerializeField] private Vector3 checkExtents = new(30f, 30f, 30f);
		[SerializeField] public float attackCooldown = 5f;
		[SerializeField] public float attackRange = 2f;
		[SerializeField] private Vector3 projectileSpawnOffset = Vector3.zero;
		[SerializeField] private InputActionAsset actions;
		[SerializeField] public List<ProjectileModifier> AvailableModifiers = new List<ProjectileModifier>();
		[SerializeField] public ProjectileModifier projectileModifier;
		private GameObject _currentModifierObject;
		private int _currentModifierIndex = 0;
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
			AvailableModifiers = GameManager.instance.AvailableModifiers;
			_moveAction = actions.FindActionMap("gameplay").FindAction("move", true);
			actions.FindActionMap("gameplay").FindAction("previous_projectile").performed += PreviousProjectile;
			actions.FindActionMap("gameplay").FindAction("next_projectile").performed += NextModifier;
			colliderComparer = new ColliderDistanceComparer(transform);
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
					if(colliderBuffer[i]!=null)	
						ShootAtEnemy(colliderBuffer[i]);
					break;
				}
			}

			CurrentModifier?.Update();
		}
		private void NextModifier(InputAction.CallbackContext obj)
		{
			if (_currentModifierIndex < AvailableModifiers.Count - 1)
			{
				_currentModifierIndex++;
				Destroy(CurrentModifier);
				CurrentModifier = Instantiate(AvailableModifiers[_currentModifierIndex]);
			}
		}

		private void PreviousProjectile(InputAction.CallbackContext obj)
		{
			if (_currentModifierIndex > 0)
			{
				_currentModifierIndex--;
				Destroy(CurrentModifier);
				CurrentModifier = Instantiate(AvailableModifiers[_currentModifierIndex]);
			}
		}

		private void CheckForEnemies()
		{
			activeColliderCount = Physics.OverlapBoxNonAlloc(checkCenter, checkExtents * attackRange, colliderBuffer, Quaternion.identity, enemyLayerMask);
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
			var projectile = projectileInstance.GetComponent<ProjectileBehavior>();
			projectile.Initialize(this, col);
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
			Gizmos.DrawWireCube(checkCenter, checkExtents * attackRange * 2);
		}
		#endif
	}
}