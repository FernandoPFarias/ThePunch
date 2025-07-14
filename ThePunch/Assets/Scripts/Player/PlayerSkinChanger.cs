using UnityEngine;

public class PlayerSkinChanger : MonoBehaviour
{
    public Renderer[] playerRenderers;
    public Material[] materials;
    private int currentMaterial = 0;

    void Start()
    {
        ApplyMaterial();
    }

    public void NextMaterial()
    {
        if (materials.Length == 0 || playerRenderers == null || playerRenderers.Length == 0)
            return;
        currentMaterial = (currentMaterial + 1) % materials.Length;
        ApplyMaterial();
    }

    public void SetMaterial(Material mat)
    {
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
        if (playerRenderers == null) return;
        foreach (var rend in playerRenderers)
        {
            if (rend != null)
                rend.material = materials[currentMaterial];
        }
    }
} 