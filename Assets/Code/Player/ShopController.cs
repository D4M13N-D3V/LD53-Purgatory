using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Purgatory.Upgrades;

namespace Purgatory.Player
{
    public class ShopController : MonoBehaviour
    {
        public List<Upgrades.UpgradeSciptableObject> Upgrades = new List<Upgrades.UpgradeSciptableObject>();
        private List<Upgrades.UpgradeSciptableObject> _currentPool;
        public GameObject ShopInterface;


        private Upgrades.UpgradeSciptableObject ItemOne;
        private Upgrades.UpgradeSciptableObject ItemTwo;
        private Upgrades.UpgradeSciptableObject ItemThree;

        public TextMeshProUGUI ItemOneText;
        public TextMeshProUGUI ItemTwoText;
        public TextMeshProUGUI ItemThreeText;

        public TextMeshProUGUI SoulsText;
        public TextMeshProUGUI CurrencyText;

        public static ShopController Instance;

        public ShopController()
        {
            Instance = this;
        }

        private void Update()
        {

            SoulsText.text = GameManager.instance.SoulAmount.ToString();
            CurrencyText.text = GameManager.instance.CurrencyAmount.ToString();
        }

        private void Start()
        {
            GameManager.instance.CurrentLevel++;
            ShopInterface.SetActive(false);
            _currentPool = Upgrades;
        }

        public void BuyItemOne()
        {
            if (ItemOne != null)
            {
                if (CurrencyController.Instance.RemoveCurrency(ItemOne.Cost))
                {
                    UpgradeController.instance.Upgrades.Add(ItemOne);
                    LeaveShop();
                }
            }
        }

        public void BuyItemTwo()
        {
            if (ItemTwo != null)
            {
                if (CurrencyController.Instance.RemoveCurrency(ItemTwo.Cost))
                {
                    UpgradeController.instance.Upgrades.Add(ItemTwo);
                    LeaveShop();
                }
            }
        }


        public void BuyItemThree()
        {
            if (ItemThree != null)
            {
                if (CurrencyController.Instance.RemoveCurrency(ItemThree.Cost))
                {
                    UpgradeController.instance.Upgrades.Add(ItemThree);
                    LeaveShop();
                }
            }
        }

        public void LeaveShop()
        {
            GameManager.instance.StartGame();
        }

        public void OpenShop()
        {
            var threeItems = Upgrades.OrderBy(x => Guid.NewGuid()).Take(3).ToArray();

            for(var i = 0; i<3; i++)
            {
                if (threeItems[i] != null)
                {
                    Upgrades.Remove(threeItems[i]);
                    switch (i)
                    {
                        case 0:
                            ItemOne = threeItems[i];
                            ItemOneText.text = $"{ItemOne.Name} - ${ItemOne.Cost}";
                            break;
                        case 1:
                            ItemTwo = threeItems[i];
                            ItemTwoText.text = $"{ItemTwo.Name} - ${ItemTwo.Cost}";
                            break;
                        case 2:
                            ItemThree = threeItems[i];
                            ItemThreeText.text = $"{ItemThree.Name} - ${ItemThree.Cost}";
                            break;
                    }
                }
            }
            ShopInterface.SetActive(true);
        }
    }
}