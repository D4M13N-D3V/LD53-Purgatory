using System.Collections;
using TMPro;
using UnityEngine;
using static Purgatory.Impl.Targetable;

namespace Purgatory.Player
{
    public class HudController : MonoBehaviour
    {
        public static HudController instance;

        private int _totalSouls = 0;
        private int _health = 0;
        private bool _canDash = false;    
        private float _attackSpeed = 0f;
        private float _attackRange = 0f;
        private float _currentSpeed = 0f;
        private float _dashSpeed = 0f;
        private float _dashLength = 0f;
        private float _dashCooldown = 0f;

        public TextMeshProUGUI HealthText;
        public TextMeshProUGUI SoulsText;
        public TextMeshProUGUI DashStatusText;
        public TextMeshProUGUI AttackSpeedText;
        public TextMeshProUGUI AttackRangeText;
        public TextMeshProUGUI CurrentSpeedText;
        public TextMeshProUGUI DashSpeedText;
        public TextMeshProUGUI DashLengthText;
        public TextMeshProUGUI DashCooldownText;


        public HudController()
        {
            if(instance==null)
                instance = this;
        }

        public void UpdateSoulStats(int totalSouls)
        {
            _totalSouls = totalSouls;
        }

        public void UpdateBoatStats(float speed, float dashSpeed, float dashLength, bool canDash, float dashCooldown)
        {
            _currentSpeed = speed;
            _dashSpeed = dashSpeed;
            _canDash = canDash;
            _dashLength = dashLength;
            _dashCooldown = dashCooldown;
        }

        public void UpdateHealth(int health)
        {
            _health = health;
        }

        public void UpdateLanternStats(float attackRange, float attackSpeed)
        {
            _attackSpeed = attackSpeed;
            _attackRange = attackRange;
        }

        public void UpdatePlayerStats(float speed, float range)
        {
            _attackRange = range;
            _attackSpeed = speed;
        }

        public void SoulCollected()
        {
            Debug.Log("Soul collected recieved on HUD.");
        }

        public void Damage(int amount)
        {
            Debug.Log("Obstacle damage recieved on HUD.");
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            HealthText.text = $"HEALTH : {_health}";
            SoulsText.text = $"SOULS : {_totalSouls}";
            DashStatusText.text = $"DASH ENABLED : {_canDash}";
            AttackSpeedText.text = $"ATTACK SPEED : {_attackSpeed}";
            AttackRangeText.text = $"ATTACK RANGE : {_attackRange}";
            CurrentSpeedText.text = $"SPEED : {_currentSpeed}";
            DashSpeedText.text = $"DASH SPEED : {_dashSpeed}";
            DashLengthText.text = $"DASH LENGTH : {_dashLength}";
            DashCooldownText.text = $"DASH COOLDOWN : {_dashCooldown}";
        }
    }
}