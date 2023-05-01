using System.Collections;
using UnityEngine;

namespace Purgatory.Player
{
    public class CurrencyController : MonoBehaviour
    {
        public static CurrencyController Instance;

        public CurrencyController()
        {
            Instance = this;
        }

        public int CurrencyAmount = 0;
        public int SoulConversionRate = 1;

        public bool RemoveCurrency(int amount)
        {
            if (CurrencyAmount >= amount)
            {
                CurrencyAmount -= amount;
                return true;
            }
            return false;
        }

        public void AddCurrency(int amount)
        {
            CurrencyAmount += amount;
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}