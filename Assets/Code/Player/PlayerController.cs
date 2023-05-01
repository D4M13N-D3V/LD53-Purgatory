using Purgatory.Impl;
using Purgatory.Player.Projectiles;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Purgatory.Player
{
    public class PlayerController : Targetable
    {
        public static PlayerController instance;
        public BoatController Boat = null;
        public ProjectileHandler Lantern = null;
        public CameraController Camera = null;
        public SoulCollectionController SoulCollector = null;

        public PlayerController()
        {
            instance = this;

        }

        public override void DeathLogic()
        {
            GameManager.instance.GameOver();
        }

        void Start()
        {
            onDamaged += HudController.instance.Damage;
            onHealed += HudController.instance.Heal;
            Upgrades.UpgradeController.instance.RefreshStats();
        }

        void Update()
        {

        }
    }

}