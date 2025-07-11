using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // arraste o Jammo_Player aqui
    public Vector3 offset = new Vector3(0, 15, -10); // ajuste para o ângulo/distance desejado
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (target == null) return;
        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.LookAt(target); // Mantém a câmera olhando para o player
    }
} 