// ===============================
// TutorialNPC.cs
// Controla o balão de fala do NPC tutorial, exibindo e ocultando mensagens.
// ===============================
using UnityEngine;
using TMPro;

public class TutorialNPC : MonoBehaviour
{
    [Header("Balão de fala")]
    [Tooltip("GameObject do balão (Canvas ou Sprite)")] public GameObject balaoFala; // GameObject do balão (Canvas ou Sprite)
    [Tooltip("Texto do balão")] public TMP_Text textoBalao;  // Texto do balão

    void Start()
    {
        // Inicializa o balão de fala desativado
        if (balaoFala != null)
            balaoFala.SetActive(false);
    }

    public void ShowDialog(string text)
    {
        // Exibe o balão de fala com o texto fornecido
        if (balaoFala != null)
            balaoFala.SetActive(true);
        if (textoBalao != null)
            textoBalao.text = text;
    }

    public void HideDialog()
    {
        // Oculta o balão de fala
        if (balaoFala != null)
            balaoFala.SetActive(false);
    }
} 