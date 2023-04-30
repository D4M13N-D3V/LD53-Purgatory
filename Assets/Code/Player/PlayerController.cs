using Purgatory.Impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Purgatory.Player
{
    public class PlayerController : Targetable
    {
        public BoatController Boat = null;
        public LanternController Lantern = null;
        public CameraController Camera = null;
        public SoulCollectionController SoulCollector = null;

        public PlayerController()
        {

        }
        public override void DeathLogic()
        {
        }

        void Start()
        {
        }

        void Update()
        {

        }
    }

}