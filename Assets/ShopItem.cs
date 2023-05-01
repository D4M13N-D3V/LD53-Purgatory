using Purgatory.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public TMPro.TextMeshProUGUI DescriptionText;
    public TMPro.TextMeshProUGUI PriceText;
    public Button BuyButton;
    public Purgatory.Upgrades.UpgradeSciptableObject Upgrade;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        DescriptionText.text = $"{Upgrade.Name} ({Upgrade.Tier})";
        PriceText.text = "$" + (Upgrade.Cost * (Upgrade.Tier + 1)).ToString();
        PriceText.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        DescriptionText.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
    }

    public void Buy()
    {
        if (CurrencyController.Instance.RemoveCurrency(Upgrade.Cost*(Upgrade.Tier+1))){
            Upgrade.Tier++;
        }
    }
}
