// ===============================
// SellZone.cs
// Zona de venda: permite ao jogador vender inimigos empilhados ao entrar na área.
// ===============================
using UnityEngine;

public class SellZone : MonoBehaviour
{
    [Header("Configuração da Zona de Venda")]
    [Tooltip("Somente este objeto ativa a venda (ex: o player)")]
    public Transform allowedObject; // Arraste o player aqui

    private void OnTriggerEnter(Collider other)
    {
        // Verifica se o objeto permitido entrou e realiza a venda
        if (allowedObject != null && other.transform != allowedObject)
            return;

        var stackManager = other.GetComponent<StackManager>();
        if (stackManager != null)
        {
            Debug.Log("[SellZone] Venda realizada!");
            int sold = stackManager.StackCount;
            stackManager.SellStack();
            if (sold > 0 && MusicManager.Instance != null && MusicManager.Instance.sellClip != null)
                MusicManager.Instance.PlaySFX(MusicManager.Instance.sellClip);
            // Chamar tutorial após venda
            if (sold > 0 && TutorialManager.Instance != null)
                TutorialManager.Instance.OnEnemySold();
        }
    }
} 