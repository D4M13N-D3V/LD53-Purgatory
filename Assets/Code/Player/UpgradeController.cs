using Purgatory.Player;
using Purgatory.Player.Projectiles;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Purgatory.Upgrades
{
    public class UpgradeController : MonoBehaviour
    {
        public List<UpgradeSciptableObject> Upgrades = new List<UpgradeSciptableObject>();

        public static UpgradeController instance;

        public UpgradeController()
        {
            if (instance == null)
                instance = this;
        }

        public void AddUpgrade(UpgradeSciptableObject upgrade)
        {
            Upgrades.Add(upgrade);
            Debug.Log($"Added upgrade {upgrade.name}");
            RefreshStats();
        }

        public void RemoveUpgradeByName(string name)
        {
            var upgrade = Upgrades.FirstOrDefault(x => x.name.Equals(name));
            if (upgrade != null)
            {
                Upgrades.Remove(upgrade);
                Debug.Log($"Removed upgrade {name}");
                RefreshStats();
            }
            else
            {
                Debug.LogWarning($"Could not remove upgrade {name} because the player does not have it.");
            }
        }


        /// <summary>
        /// Removes all non-permanant upgrades from the player.
        /// </summary>
        public void WipeNonPermanantUpgrades()
        {
            var upgradesToRemove = Upgrades.Where(x => !x.IsPermanant).ToList();
            foreach (var upgrade in upgradesToRemove)
            {
                Upgrades.Remove(upgrade);
            }
            RefreshStats();
        }

        /// <summary>
        /// Removes all upgrades from the player.
        /// </summary>
        public void RefreshStats()
        {
            // hard code base stats lol
            var baseSpeed = 1f;
            var baseDashCooldown = 3f;
            var baseDashLength = 2f;
            var baseDeceleration = 0.7f;
            var baseDashMultiplier = 1.5f;

            var baseMaximumHealth = 1;
            
            var baseAttackCooldown = 1f;
            var baseAttackRange = 1f;

            var baseCollectionRadius = 1f;


            // Make temp vars for total stat multipliers
            var totalHealthModifier = 0;
            var totalMovementSpeedModifier = 1f;
            var totalDashCooldownModifier = 1f;
            var totalDashLengthModifier = 1f;
            var totalMovementDecelerationModifier = 1f;
            var totalDashSpeedModifier = 1f;
            var totalAttackSpeedModifier = 1f;
            var totalAttackRangeModifier = 1f;
            var totalSoulCollectionRadius = 1f;



            //modify stats

            foreach(var upgrade in Upgrades)
            {
                totalHealthModifier += upgrade.HealthModifier;
                totalMovementSpeedModifier += upgrade.MovementSpeedModifier;
                totalDashCooldownModifier += upgrade.DashCooldownModifier;
                totalDashLengthModifier += upgrade.DashLengthModifier;
                totalMovementDecelerationModifier += upgrade.MovementDecelerationModifier;
                totalDashSpeedModifier += upgrade.DashSpeedModifier;
                totalAttackSpeedModifier += upgrade.AttackSpeedModifier;
                totalAttackRangeModifier += upgrade.AttackRangeModifier;
                totalSoulCollectionRadius += upgrade.SoulCollectionRadius;
            }

            // Apply stat multipliers
            
            //boat stats
            BoatController.instance.Speed =  baseSpeed *  totalMovementSpeedModifier;
            BoatController.instance.DashCooldown = baseDashCooldown * totalDashCooldownModifier;
            BoatController.instance.DashLength = baseDashLength * totalDashLengthModifier;
            BoatController.instance.Deceleration = baseDeceleration * totalMovementDecelerationModifier;
            BoatController.instance.DashMultiplier = baseDashMultiplier * totalDashSpeedModifier;

            //projectile stats
            ProjectileHandler.instance.attackCooldown = baseAttackCooldown * totalAttackSpeedModifier;
            ProjectileHandler.instance.attackRange = baseAttackRange * totalAttackRangeModifier;
            
            //player stats
            PlayerController.instance.SetMaximumHealth(baseMaximumHealth + totalHealthModifier);

            //soul stats
            SoulCollectionController.instance.CollectionRadius = baseCollectionRadius * totalSoulCollectionRadius;


            var projectiles = Upgrades.Where(x => x.Projectile != null).Select(x => x.Projectile).ToList(); ;
            ProjectileHandler.instance.AvailableProjectiles = projectiles;
            GameManager.instance.AvailableProjectiles = projectiles;
        }
    }
}