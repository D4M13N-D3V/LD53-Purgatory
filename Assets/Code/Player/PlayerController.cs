using Purgatory.Impl;
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
        public LanternController Lantern = null;
        public CameraController Camera = null;
        public SoulCollectionController SoulCollector = null;

        public PlayerController()
        {
            if (instance == null)
                instance = this;
        }

        public override void DeathLogic()
        {
        }

        void Start()
        {
            onDamaged += HudController.instance.Damage;
            onHealed += HudController.instance.Heal;
        }

        void Update()
        {

        }
    }

}