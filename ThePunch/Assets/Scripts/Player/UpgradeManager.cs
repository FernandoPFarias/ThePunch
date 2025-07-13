using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public StackManager stackManager;
    public PlayerSkinChanger skinChanger;
    public CapacityBarUI capacityBarUI; // Adicionado para integração
    public int capacityUpgradeCost = 50;
    public int colorUpgradeCost = 30;
    public Button capacityButton;
    public Button colorButton;
    public TMPro.TMP_Text capacityCostText;
    public TMPro.TMP_Text colorCostText;
    public TMPro.TMP_Text stackInfoText;

    void Start()
    {
        if (capacityButton != null)
            capacityButton.onClick.AddListener(UpgradeCapacity);
        if (colorButton != null)
            colorButton.onClick.AddListener(UpgradeColor);
        UpdateUI();
        if (stackManager != null)
            stackManager.OnMoneyChanged += _ => UpdateUI();
        if (stackManager != null)
            stackManager.OnStackChanged += _ => UpdateUI();
    }

    void OnDestroy()
    {
        if (capacityButton != null)
            capacityButton.onClick.RemoveListener(UpgradeCapacity);
        if (colorButton != null)
            colorButton.onClick.RemoveListener(UpgradeColor);
        if (stackManager != null)
            stackManager.OnMoneyChanged -= _ => UpdateUI();
        if (stackManager != null)
            stackManager.OnStackChanged -= _ => UpdateUI();
    }

    void UpgradeCapacity()
    {
        if (stackManager.money >= capacityUpgradeCost)
        {
            stackManager.money -= capacityUpgradeCost;
            stackManager.maxStack++;
            stackManager.OnMoneyChanged?.Invoke(stackManager.money);
            UpdateUI();
            stackManager.UpdateCapacityBar(); // Garante atualização imediata da UI
        }
    }

    void UpgradeColor()
    {
        if (stackManager.money >= colorUpgradeCost && skinChanger != null)
        {
            stackManager.money -= colorUpgradeCost;
            skinChanger.NextMaterial();
            stackManager.OnMoneyChanged?.Invoke(stackManager.money);
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        if (capacityCostText != null)
            capacityCostText.text = " " + capacityUpgradeCost;
        if (colorCostText != null)
            colorCostText.text = " " + colorUpgradeCost;
        if (stackInfoText != null && stackManager != null)
            stackInfoText.text = $"Capacidade: {stackManager.StackCount}/{stackManager.maxStack}";
        if (capacityBarUI != null && stackManager != null)
            capacityBarUI.SetCapacity(stackManager.maxStack, stackManager.maxPossibleCapacity);
    }
} 