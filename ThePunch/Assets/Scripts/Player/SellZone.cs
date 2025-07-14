using UnityEngine;

public class SellZone : MonoBehaviour
{
    [Header("Somente este objeto ativa a venda")]
    public Transform allowedObject; // Arraste o player aqui

    private void OnTriggerEnter(Collider other)
    {
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
        }
    }
} 