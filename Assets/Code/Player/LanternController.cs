using Purgatory.Impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Purgatory.Interfaces;
using Purgatory.Enemy;
using UnityEngine.InputSystem;

namespace Purgatory.Player
{
    public class LanternController : Attacker
    {
        public static LanternController instance;
        
        
        internal List<GameObject> _availableProjectiles = new List<GameObject>();

        [SerializeField]
        public InputActionAsset actions;
        private InputAction _moveAction;

        [SerializeField]
        private GameObject _currentProjectile;
        private int _currentProjectileIndex = 0;

        public LanternController()
        {
            if(instance==null)
                instance = this;
        }

        public void SetAvailableProjectiles(List<GameObject> projectiles)
        {
            _availableProjectiles = projectiles;
            if (_currentProjectile == null && _availableProjectiles.Any())
            {
                _currentProjectileIndex = 0;
                _currentProjectile = _availableProjectiles.First();
            }

        }

        private void NextProjectile(InputAction.CallbackContext obj)
        {
            if (_currentProjectileIndex == _availableProjectiles.Count()-1)
            {
                var first = _availableProjectiles.First();
                _currentProjectileIndex = 0;
                _currentProjectile = first;
            }
            else
            {
                _currentProjectileIndex++;
                _currentProjectile = _availableProjectiles[_currentProjectileIndex];
            }
        }

        private void PreviousProjectile(InputAction.CallbackContext obj)
        {
            if (_currentProjectileIndex == 0)
            {
                var last = _availableProjectiles.Last();
                _currentProjectileIndex = _availableProjectiles.IndexOf(last);
                _currentProjectile = last;
            }
            else
            {
                _currentProjectileIndex--;
                _currentProjectile = _availableProjectiles[_currentProjectileIndex];
            }
        }

        // Update is called once per frame
        void Update()
        {
        }

        private void Awake()
        {
            _moveAction = actions.FindActionMap("gameplay").FindAction("move", true);
            actions.FindActionMap("gameplay").FindAction("previous_projectile").performed += PreviousProjectile;
            actions.FindActionMap("gameplay").FindAction("next_projectile").performed += NextProjectile;
            StartCoroutine(UpdateHud());
        }

        private IEnumerator UpdateHud()
        {
            HudController.instance.UpdatePlayerStats(AttackInterval, AttackRange, _currentProjectile);
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(UpdateHud());
        }


        public override void LaunchProjectile()
        {
            if (_targetTransform != null)
            {
                var rotation = Quaternion.LookRotation(_targetTransform.position - transform.position);
                var newProject = GameObject.Instantiate(_currentProjectile, transform.position, rotation);
                newProject.transform.rotation = rotation;
                var projectile = newProject.GetComponent<Projectile>();
                projectile.TargetLocation = _targetTransform.position;
                Debug.Log("Player projectile launched!");
            }
        }

        public override GameObject GetTarget()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, AttackRange);
            var target = hitColliders.Where(x=> x.GetComponent<EnemyController>()!=null).OrderBy(x => Guid.NewGuid()).FirstOrDefault();

            if (target != null)
                Debug.Log($"Lantern targeting gameobject {target.gameObject.name}");

            return target?.gameObject;
        }
    }
}