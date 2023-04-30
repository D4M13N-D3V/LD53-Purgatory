using Assets.Code.Player;
using Purgatory.Interfaces;
using System.Collections;
using UnityEngine;

namespace Purgatory.Impl
{
    public abstract class Soul : MonoBehaviour, ISoul
    {
        [SerializeField]
        private int _amount = 1;

        public int Amount => _amount;

        public abstract void Collected();

        public void Reward(SoulCollectionController soulCollectionController)
        {
            soulCollectionController.AddSoul(Amount);
        }
        private void OnCollisionEnter(Collision collision)
        {
            var collector = collision.gameObject.GetComponent<SoulCollectionController>();
            Reward(collector);
            Collected();
        }
    }
}