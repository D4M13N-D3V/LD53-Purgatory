using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Purgatory.Upgrades;
using UnityEngine.UI;

namespace Purgatory.Player
{
    public class ShopController : MonoBehaviour
    {
        public List<Upgrades.UpgradeSciptableObject> Upgrades = new List<Upgrades.UpgradeSciptableObject>();
        public TextMeshProUGUI SoulsText;
        public TextMeshProUGUI CurrencyText;
        public Transform Souls;
        public Transform Currency;
        public GameObject ShopItemPrefab;
        public Button BackButton;
        public static ShopController Instance;
        public Transform ShopScrollView;
        public Transform Menu;
        public float xOffset = 16.0f;
        public float yOffset = 0f;
        public float ySpacing = 65f;

        public ShopController()
        {
            Instance = this;
        }

        private void Update()
        {

            SoulsText.text = GameManager.instance.SoulAmount.ToString();
            CurrencyText.text = GameManager.instance.CurrencyAmount.ToString();
        }

        public void CloseShop()
        {
            BackButton.gameObject.SetActive(false);
            Souls.gameObject.SetActive(false);
            Currency.gameObject.SetActive(false);
            ShopScrollView.gameObject.SetActive(false);
            Menu.gameObject.SetActive(true);
        }

        public void OpenShop()
        {
            BackButton.gameObject.SetActive(true);
            Souls.gameObject.SetActive(true);
            Currency.gameObject.SetActive(true);
            ShopScrollView.gameObject.SetActive(true);
            Menu.gameObject.SetActive(false);
        }
        private void Start()
        {
            CloseShop();
            var total = 0;
            foreach(var upgrade in Upgrades)
            {
                var name = $"{upgrade.name} ({upgrade.Tier})";
                var gameObj = Instantiate(ShopItemPrefab, ShopScrollView);
                gameObj.GetComponent<ShopItem>().Upgrade = upgrade;
                gameObj.name = name;
                gameObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(xOffset, yOffset - (total * ySpacing));
                total++;
            }
        }

    }
}