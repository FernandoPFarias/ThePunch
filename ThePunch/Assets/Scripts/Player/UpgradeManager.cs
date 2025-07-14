using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class PlayerUpgradeLevel
{
    public int capacity;
    public Material skinMaterial;
    public int upgradeCost;
}

public class UpgradeManager : MonoBehaviour
{
    public StackManager stackManager;
    public PlayerSkinChanger skinChanger;
    public CapacityBarUI capacityBarUI;
    public Button upgradeButton;
    public TMPro.TMP_Text upgradeCostText;
    public TMPro.TMP_Text stackInfoText;
    public List<PlayerUpgradeLevel> upgradeLevels;
    public Animator upgradeButtonAnimator;
    private int currentLevel = 0;

    void Start()
    {
        if (upgradeButton != null)
            upgradeButton.onClick.AddListener(UpgradePlayer);
        UpdateUI();
        if (stackManager != null)
            stackManager.OnMoneyChanged += _ => UpdateUI();
        if (stackManager != null)
            stackManager.OnStackChanged += _ => UpdateUI();
    }

    void OnDestroy()
    {
        if (upgradeButton != null)
            upgradeButton.onClick.RemoveListener(UpgradePlayer);
        if (stackManager != null)
            stackManager.OnMoneyChanged -= _ => UpdateUI();
        if (stackManager != null)
            stackManager.OnStackChanged -= _ => UpdateUI();
    }

    void UpgradePlayer()
    {
        if (upgradeButtonAnimator != null)
            upgradeButtonAnimator.SetTrigger("T_isPressed");
        if (currentLevel >= upgradeLevels.Count)
            return;
        var level = upgradeLevels[currentLevel];
        if (stackManager.money >= level.upgradeCost)
        {
            stackManager.money -= level.upgradeCost;
            ApplyUpgrade(level);
            currentLevel++;
            if (MusicManager.Instance != null && MusicManager.Instance.upgradeClip != null)
                MusicManager.Instance.PlaySFX(MusicManager.Instance.upgradeClip);
            stackManager.OnMoneyChanged?.Invoke(stackManager.money);
            UpdateUI();
            stackManager.UpdateCapacityBar();
        }
    }

    void ApplyUpgrade(PlayerUpgradeLevel level)
    {
        if (stackManager != null)
            stackManager.maxStack = level.capacity;
        if (skinChanger != null && level.skinMaterial != null)
            skinChanger.SetMaterial(level.skinMaterial);
    }

    void UpdateUI()
    {
        if (upgradeCostText != null)
        {
            if (currentLevel < upgradeLevels.Count)
                upgradeCostText.text = " " + upgradeLevels[currentLevel].upgradeCost;
            else
                upgradeCostText.text = "Max";
        }
        if (stackInfoText != null && stackManager != null)
            stackInfoText.text = $"{stackManager.StackCount}/{stackManager.maxStack}";
        if (capacityBarUI != null && stackManager != null)
            capacityBarUI.SetCapacity(stackManager.maxStack, stackManager.maxPossibleCapacity);
    }
} 