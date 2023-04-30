using Purgatory.Interfaces;
using Purgatory.Player;
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
            if (collector != null)
            {
                Reward(collector);
                Collected();
            }
        }
    }
}