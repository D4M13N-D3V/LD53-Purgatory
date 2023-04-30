using Assets.Code.Player;
using UnityEditor;
using UnityEngine;

namespace Purgatory.Interfaces
{
    public interface ISoul
    {
        public int Amount { get; }
        public void Collected();
        public void Reward(SoulCollectionController soulCollectionController);   
    }
}