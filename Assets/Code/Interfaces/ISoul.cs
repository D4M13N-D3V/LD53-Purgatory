using Purgatory.Player;
using UnityEditor;
using UnityEngine;

namespace Purgatory.Interfaces
{
    public interface ISoul
    {
        public int Amount { get; }
        public void CleanupAfterCollection();
        public void Collect(SoulCollectionController soulCollectionController);   
    }
}