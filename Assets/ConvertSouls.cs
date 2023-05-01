using Purgatory.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvertSouls : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var souls = GameManager.instance.SoulAmount;
        var convertedAmount = souls * CurrencyController.Instance.SoulConversionRate;
        CurrencyController.Instance.AddCurrency(convertedAmount);
        GameManager.instance.SoulAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
