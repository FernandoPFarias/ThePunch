using UnityEngine;
using TMPro;

public class MoneyUI : MonoBehaviour
{
    public TMP_Text moneyText;
    public StackManager stackManager;

    void Start()
    {
        if (stackManager != null)
        {
            stackManager.OnMoneyChanged += UpdateMoney;
            UpdateMoney(stackManager.money);
        }
    }

    void OnDestroy()
    {
        if (stackManager != null)
            stackManager.OnMoneyChanged -= UpdateMoney;
    }

    void UpdateMoney(int value)
    {
        if (moneyText != null)
            moneyText.text = "" + value;
    }
} 