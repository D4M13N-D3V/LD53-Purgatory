using Purgatory.Player;
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

        public void RefreshStats()
        {
            BoatController.instance.Speed = 0;
            BoatController.instance.DashCooldown = 0;
            BoatController.instance.DashLength = 0;
            BoatController.instance.Deceleration = 0;
            BoatController.instance.DashMultiplier = 0;

            PlayerController.instance.SetMaximumHealth(1);
            
            LanternController.instance.AttackInterval = 0;
            LanternController.instance.AttackRange = 0;

            SoulCollectionController.instance.CollectionRadius = 0;

            foreach(var upgrade in Upgrades)
            {
                BoatController.instance.Speed += upgrade.MovementSpeedModifier;
                BoatController.instance.DashCooldown += upgrade.DashCooldownModifier;
                BoatController.instance.DashLength += upgrade.DashLengthModifier;
                BoatController.instance.Deceleration += upgrade.MovementDecelerationModifier;
                BoatController.instance.DashMultiplier += upgrade.DashSpeedModifier;

                var maximumhealth = PlayerController.instance.GetMaximumHealth() + upgrade.HealthModifier;
                PlayerController.instance.SetMaximumHealth(maximumhealth);
                PlayerController.instance.Heal(maximumhealth);

                LanternController.instance.AttackInterval += upgrade.AttackSpeedModifier;
                LanternController.instance.AttackRange += upgrade.AttackRangeModifier;

                SoulCollectionController.instance.CollectionRadius += upgrade.SoulCollectionRadius;
            }

            var projectiles = Upgrades.Where(x => x.Projectile != null).Select(x => x.Projectile).ToList(); ;
            LanternController.instance.SetAvailableProjectiles(projectiles);
        }

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}