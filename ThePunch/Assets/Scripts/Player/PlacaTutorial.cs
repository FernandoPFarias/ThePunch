// ===============================
// PlacaTutorial.cs
// Controla a placa 3D de tutorial, exibindo e limpando mensagens para o jogador.
// ===============================
using UnityEngine;
using TMPro;

public class PlacaTutorial : MonoBehaviour
{
    [Header("Referências de UI")]
    [Tooltip("Componente TMP_Text da placa de tutorial")] public TMP_Text textoPlaca; // Arraste aqui o TextMeshProUGUI do Canvas da placa

    public void SetTexto(string texto)
    {
        // Define o texto da placa de tutorial
        if (textoPlaca != null)
            textoPlaca.text = texto;
    }

    public void Limpar()
    {
        // Limpa o texto da placa de tutorial
        if (textoPlaca != null)
            textoPlaca.text = "";
    }
} 