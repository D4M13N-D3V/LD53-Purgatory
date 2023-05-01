using Purgatory.Enums;
using System.Collections;
using UnityEngine;

namespace Purgatory.Upgrades
{
    [CreateAssetMenu(fileName = "Upgrade", menuName = "Purgatory/CreateUpgrade")]
    public class UpgradeSciptableObject : ScriptableObject
    {
        public string Name;
        public int HealthModifier;
        public float AttackSpeedModifier;
        public float AttackRangeModifier;
        public float MovementSpeedModifier;
        public float MovementDecelerationModifier;
        public float DashSpeedModifier;
        public float DashLengthModifier;
        public float DashCooldownModifier;
        public float SoulCollectionRadius;
        public GameObject Projectile;
        public EnumUpgradeType Type;
    }
}