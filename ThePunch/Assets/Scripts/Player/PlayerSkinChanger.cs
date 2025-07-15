// ===============================
// PlayerSkinChanger.cs
// Permite trocar o material (skin) do jogador, aplicando em todos os renderers.
// ===============================
using UnityEngine;

public class PlayerSkinChanger : MonoBehaviour
{
    [Header("Referências de Renderers e Materiais")]
    [Tooltip("Renderers do jogador para trocar o material")] public Renderer[] playerRenderers;
    [Tooltip("Materiais disponíveis para troca de skin")] public Material[] materials;
    private int currentMaterial = 0;

    void Start()
    {
        // Aplica o material inicial ao jogador
        ApplyMaterial();
    }

    public void NextMaterial()
    {
        // Troca para o próximo material disponível
        if (materials.Length == 0 || playerRenderers == null || playerRenderers.Length == 0)
            return;
        currentMaterial = (currentMaterial + 1) % materials.Length;
        ApplyMaterial();
    }

    public void SetMaterial(Material mat)
    {
        // Aplica o material fornecido em todos os renderers
        if (playerRenderers == null || mat == null) return;
        foreach (var rend in playerRenderers)
        {
            if (rend != null)
            {
                // Se o renderer tem múltiplos materiais, substitui todos
                var mats = rend.sharedMaterials;
                for (int i = 0; i < mats.Length; i++)
                {
                    mats[i] = mat;
                }
                rend.materials = mats;
            }
        }
    }

    private void ApplyMaterial()
    {
        // Aplica o material atual em todos os renderers
        if (playerRenderers == null) return;
        foreach (var rend in playerRenderers)
        {
            if (rend != null)
                rend.material = materials[currentMaterial];
        }
    }
} 