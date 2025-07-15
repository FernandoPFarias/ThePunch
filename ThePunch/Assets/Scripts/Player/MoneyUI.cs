// ===============================
// MoneyUI.cs
// Responsável por exibir o valor de dinheiro na UI usando TextMesh Pro, ouvindo eventos do StackManager.
// ===============================
using UnityEngine;
using TMPro;

public class MoneyUI : MonoBehaviour
{
    [Header("Referências de UI")]
    [Tooltip("Componente TMP_Text que exibe o dinheiro")] public TMP_Text moneyText;
    [Header("Referências de Sistema")]
    [Tooltip("Referência ao StackManager para ouvir eventos de dinheiro")] public StackManager stackManager;

    void Start()
    {
        // Assina o evento de mudança de dinheiro e atualiza a UI
        if (stackManager != null)
        {
            stackManager.OnMoneyChanged += UpdateMoney;
            UpdateMoney(stackManager.money);
        }
    }

    void OnDestroy()
    {
        // Remove a assinatura do evento ao destruir
        if (stackManager != null)
            stackManager.OnMoneyChanged -= UpdateMoney;
    }

    void UpdateMoney(int value)
    {
        // Atualiza o texto da UI com o valor atual de dinheiro
        if (moneyText != null)
            moneyText.text = "" + value;
    }
} 