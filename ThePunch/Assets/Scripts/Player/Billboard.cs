// ===============================
// Billboard.cs
// Faz o objeto sempre olhar para a câmera principal (efeito billboard).
// ===============================
using UnityEngine;

public class Billboard : MonoBehaviour
{
    void LateUpdate()
    {
        // Rotaciona o objeto para olhar para a câmera principal
        if (Camera.main != null)
            transform.LookAt(transform.position + Camera.main.transform.forward);
    }
} 