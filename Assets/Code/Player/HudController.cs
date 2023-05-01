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


        public HudController()
        {
            if(instance==null)
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
            AttackSpeedText.text = $"ATTACK SPEED : {LanternController.instance.AttackInterval}";
            AttackRangeText.text = $"ATTACK RANGE : {LanternController.instance.AttackRange}";
            CurrentSpeedText.text = $"SPEED : {BoatController.instance.Speed}";
            DashSpeedText.text = $"DASH SPEED : {BoatController.instance.DashMultiplier}";
            DashLengthText.text = $"DASH LENGTH : {BoatController.instance.DashLength}";
            DashCooldownText.text = $"DASH COOLDOWN : {BoatController.instance.DashCooldown}";
            DecelerationCooldownText.text = $"DECELERATION : {BoatController.instance.Deceleration}";
            if(_currentProjectile!=null)
                CurrentProjectileText.text = $"CURRENT PROJECTILE : {_currentProjectile?.gameObject.name}";
        }
    }
}