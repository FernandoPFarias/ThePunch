// ===============================
// CameraFollow.cs
// Faz a câmera seguir suavemente o jogador, mantendo um offset e olhando para o alvo.
// ===============================
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Configuração de Seguimento")]
    [Tooltip("Transform do alvo a ser seguido (ex: jogador)")]
    public Transform target; // arraste o Jammo_Player aqui
    [Tooltip("Offset da câmera em relação ao alvo")]
    public Vector3 offset = new Vector3(0, 15, -10); // ajuste para o ângulo/distance desejado
    [Tooltip("Velocidade de suavização do movimento da câmera")]
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        // Atualiza a posição e rotação da câmera para seguir o alvo suavemente
        if (target == null) return;
        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.LookAt(target); // Mantém a câmera olhando para o player
    }
} 