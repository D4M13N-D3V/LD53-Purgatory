using Purgatory.Player.Projectiles;
using System.Collections;
using TMPro;
using UnityEngine;
using static Purgatory.Impl.Targetable;

namespace Purgatory.Player
{
    public class HudController : MonoBehaviour
    {
        public static HudController instance;
        private GameObject _currentProjectile;

        public TextMeshProUGUI HealthText;
        public TextMeshProUGUI SoulsText;
        public TextMeshProUGUI DashStatusText;
        public TextMeshProUGUI AttackSpeedText;
        public TextMeshProUGUI AttackRangeText;
        public TextMeshProUGUI CurrentSpeedText;
        public TextMeshProUGUI DashSpeedText;
        public TextMeshProUGUI DashLengthText;
        public TextMeshProUGUI DashCooldownText;
        public TextMeshProUGUI DecelerationCooldownText;
        public TextMeshProUGUI CurrentProjectileText;
        public TextMeshProUGUI SoulConversionText;
        public TextMeshProUGUI SoulRetentionText;
        public TextMeshProUGUI CurrencyText;


        public HudController()
        {
                instance = this;
        }

        public void SoulCollected()
        {
            Debug.Log("Soul collected recieved on HUD.");
        }

        public void Damage(int amount)
        {
            Debug.Log("Obstacle damage recieved on HUD.");
        }

        public void Heal(int amount)
        {
            Debug.Log("Healed recieved on HUD");
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            HealthText.text = $"HEALTH : {PlayerController.instance.CurrentHP}";
            SoulsText.text = $"SOULS : {SoulCollectionController.instance.Souls}";
            DashStatusText.text = $"DASHING : {BoatController.instance.Dashing}";
            AttackSpeedText.text = $"ATTACK SPEED : {ProjectileHandler.instance.attackCooldown}";
            AttackRangeText.text = $"ATTACK RANGE : {ProjectileHandler.instance.attackCooldown}";
            CurrentSpeedText.text = $"SPEED : {BoatController.instance.Speed}";
            DashSpeedText.text = $"DASH SPEED : {BoatController.instance.DashMultiplier}";
            DashLengthText.text = $"DASH LENGTH : {BoatController.instance.DashLength}";
            DashCooldownText.text = $"DASH COOLDOWN : {BoatController.instance.DashCooldown}";
            DecelerationCooldownText.text = $"DECELERATION : {BoatController.instance.Deceleration}";
            SoulRetentionText.text = $"SOUL RETENTION : {SoulCollectionController.instance.SoulRetentionRate*100}%";
            SoulConversionText.text = $"SOUL CONVERSION : {CurrencyController.Instance.SoulConversionRate}x";
            CurrencyText.text = $"CURRENCY : {CurrencyController.Instance.CurrencyAmount}";
            if (_currentProjectile!=null)
                CurrentProjectileText.text = $"CURRENT PROJECTILE : {ProjectileHandler.instance._currentProjectile.name}";
        }
    }
}