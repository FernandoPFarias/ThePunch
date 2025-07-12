using UnityEngine;

public class StackableCharacter : MonoBehaviour
{
    // Script marcador para facilitar busca e integração
    public bool isStacked = false;
    public bool canBeCollected = false;
    [Header("Coleta")]
    public float collectDelay = 0.5f;
} 