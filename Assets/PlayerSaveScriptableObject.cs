using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets
{
    public class PlayerSaveScriptableObject : ScriptableObject
    {
        public List<Purgatory.Upgrades.UpgradeSciptableObject> Upgrades = new List<Purgatory.Upgrades.UpgradeSciptableObject>();
        public int Souls = 0;
        public int CurrencyAmount = 0;
        public int CurrentLevel = 0;
        public int SoulAmount = 0;
    }
}