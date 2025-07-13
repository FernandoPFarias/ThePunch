using UnityEngine;
using UnityEngine.UI;

public class CapacityBarUI : MonoBehaviour
{
    [Header("Barra de Capacidade (Fundo)")]
    public Image capacityBar; // Barra de fundo, mostra a capacidade máxima (Image Type = Filled)
    [Header("Barra de Carregando (Frente)")]
    public Image loadingBar; // Barra da frente, mostra quanto está carregando (Image Type = Filled)

    private int maxPossibleCapacity = 1;
    private int currentMaxCapacity = 1;
    private int currentLoad = 0;

    // Chame este método para atualizar a capacidade máxima e o limite possível
    public void SetCapacity(int newMaxCapacity, int newMaxPossibleCapacity)
    {
        currentMaxCapacity = Mathf.Clamp(newMaxCapacity, 1, newMaxPossibleCapacity);
        maxPossibleCapacity = Mathf.Max(1, newMaxPossibleCapacity);
        UpdateBars();
    }

    // Chame este método para atualizar a quantidade carregada
    public void SetLoad(int newLoad)
    {
        currentLoad = Mathf.Clamp(newLoad, 0, currentMaxCapacity);
        UpdateBars();
    }

    private void UpdateBars()
    {
        if (capacityBar != null && maxPossibleCapacity > 0)
            capacityBar.fillAmount = (float)currentMaxCapacity / maxPossibleCapacity;
        int clampedLoad = Mathf.Clamp(currentLoad, 0, currentMaxCapacity);
        if (loadingBar != null && maxPossibleCapacity > 0)
            loadingBar.fillAmount = (float)clampedLoad / maxPossibleCapacity;
    }
} 