using UnityEngine;

public class BalaoFollower : MonoBehaviour
{
    public Transform anchor; // Arraste aqui o BalaoAnchor do NPC

    void LateUpdate()
    {
        if (anchor != null)
        {
            transform.position = anchor.position;
        }
    }
} 