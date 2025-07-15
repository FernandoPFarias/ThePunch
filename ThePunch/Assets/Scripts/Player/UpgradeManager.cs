// ===============================
// UpgradeManager.cs
// Gerencia upgrades do jogador: capacidade, skin, velocidade e custo. Atualiza UI e aplica upgrades.
// ===============================
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class PlayerUpgradeLevel
{
    public int capacity;
    public Material skinMaterial;
    public int upgradeCost;
    public float moveSpeed = 5f; // NOVO: velocidade de movimento para este nível
}

public class UpgradeManager : MonoBehaviour
{
    [Header("Referências de Sistema")]
    [Tooltip("Referência ao StackManager do jogador")] public StackManager stackManager;
    [Tooltip("Script para trocar a skin do jogador")] public PlayerSkinChanger skinChanger;
    [Tooltip("UI da barra de capacidade")] public CapacityBarUI capacityBarUI;
    [Header("Referências de UI")]
    [Tooltip("Botão de upgrade")] public Button upgradeButton;
    [Tooltip("Texto do custo de upgrade (TMP)")] public TMPro.TMP_Text upgradeCostText;
    [Tooltip("Texto de info da stack (TMP)")] public TMPro.TMP_Text stackInfoText;
    [Header("Configuração de Upgrades")]
    [Tooltip("Lista de níveis de upgrade do jogador")] public List<PlayerUpgradeLevel> upgradeLevels;
    [Tooltip("Animator do botão de upgrade")] public Animator upgradeButtonAnimator;
    [Tooltip("Referência ao PlayerController para alterar velocidade")] public PlayerController playerController;
    private int currentLevel = 0;

    void Start()
    {
        // Assina eventos e inicializa UI
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
        // Remove assinaturas de eventos
        if (upgradeButton != null)
            upgradeButton.onClick.RemoveListener(UpgradePlayer);
        if (stackManager != null)
            stackManager.OnMoneyChanged -= _ => UpdateUI();
        if (stackManager != null)
            stackManager.OnStackChanged -= _ => UpdateUI();
    }

    void UpgradePlayer()
    {
        // Aplica upgrade se houver dinheiro suficiente e atualiza UI
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
            // Chamar tutorial após upgrade
            if (TutorialManager.Instance != null)
                TutorialManager.Instance.OnUpgrade();
        }
    }

    void ApplyUpgrade(PlayerUpgradeLevel level)
    {
        // Aplica os valores do upgrade ao jogador
        if (stackManager != null)
            stackManager.maxStack = level.capacity;
        if (skinChanger != null && level.skinMaterial != null)
            skinChanger.SetMaterial(level.skinMaterial);
        if (playerController != null)
            playerController.moveSpeed = level.moveSpeed; // NOVO: aplica velocidade
    }

    void UpdateUI()
    {
        // Atualiza textos e barra de capacidade na UI
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