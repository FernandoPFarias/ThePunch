using System.Collections.Generic;
using UnityEngine;

public class StackManager : MonoBehaviour
{
    public Transform stackOrigin; // Ponto de origem da pilha nas costas do player
    public float stackSpacing = 1.2f; // Espaço vertical entre inimigos
    public float followSpeed = 10f; // Velocidade de suavização/inércia
    public float pickupRange = 2.5f; // Range para pegar inimigos derrotados
    public LayerMask enemyLayer;

    [Header("Configuração de Empilhamento")]
    public Vector3 stackDirection = Vector3.up; // Direção customizável para empilhar (ajustável no Inspector)

    [Header("Empilhamento com Prefab")]
    public GameObject stackedEnemyPrefab; // Prefab a ser instanciado na pilha

    [Header("Ajustes Visuais da Pilha")]
    public float stackYOffset = 0f; // Offset vertical para ajustar altura dos empilhados
    public Vector3 stackedScale = Vector3.one; // Escala dos empilhados

    [Header("Inércia da Pilha")]
    public float spring = 0.15f; // Quanto menor, mais elástico
    public float damping = 0.8f; // (Reservado para uso futuro, se quiser amortecer mais)

    private List<GameObject> stackedPrefabs = new List<GameObject>();
    private List<Vector3> targetPositions = new List<Vector3>();
    private List<Vector3> velocities = new List<Vector3>();

    void Update()
    {
        // Empilhamento automático de inimigos derrotados
        Collider[] hits = Physics.OverlapSphere(stackOrigin.position, pickupRange, enemyLayer);
        Debug.Log($"[StackManager] Detectados {hits.Length} inimigos no range de pickup.");
        foreach (var hit in hits)
        {
            var stackable = hit.GetComponentInParent<StackableCharacter>();
            var enemyController = hit.GetComponentInParent<EnemyController>();
            Debug.Log($"[StackManager] Checando: {hit.name} | Stackable: {stackable != null} | Ragdoll: {(enemyController != null && enemyController.IsRagdollActive())} | isStacked: {(stackable != null ? stackable.isStacked.ToString() : "-")}");
            if (stackable != null && enemyController != null && enemyController.IsRagdollActive() && !stackable.isStacked && stackable.canBeCollected)
            {
                Debug.Log($"[StackManager] Empilhando: {hit.name}");
                AddToStack(stackable.transform);
                stackable.isStacked = true;
            }
        }
        // Efeito de inércia tipo SmoothDamp
        while (velocities.Count < stackedPrefabs.Count)
            velocities.Add(Vector3.zero);
        while (velocities.Count > stackedPrefabs.Count)
            velocities.RemoveAt(velocities.Count - 1);
        Vector3 dir = stackDirection.normalized;
        Vector3 anchor = stackOrigin.position;
        for (int i = 0; i < stackedPrefabs.Count; i++)
        {
            Vector3 targetPos = stackOrigin.position + stackDirection.normalized * stackSpacing * i + Vector3.up * stackYOffset;
            Vector3 velocity = velocities[i];
            stackedPrefabs[i].transform.position = Vector3.SmoothDamp(
                stackedPrefabs[i].transform.position,
                targetPos,
                ref velocity,
                spring,
                Mathf.Infinity,
                Time.deltaTime
            );
            velocities[i] = velocity;
            stackedPrefabs[i].transform.rotation = Quaternion.LookRotation(stackDirection.normalized);
            stackedPrefabs[i].transform.localScale = stackedScale;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (stackOrigin != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(stackOrigin.position, pickupRange);
        }
        else
        {
            Debug.LogWarning("[StackManager] stackOrigin não está atribuído!");
        }
    }

    public void AddToStack(Transform enemy)
    {
        // Instancia o prefab na posição da pilha
        if (stackedEnemyPrefab != null)
        {
            GameObject stacked = Instantiate(stackedEnemyPrefab);
            // Zera transformações herdadas do prefab
            stacked.transform.position = Vector3.zero;
            stacked.transform.rotation = Quaternion.identity;
            stacked.transform.localScale = Vector3.one;
            // Agora aplica os valores desejados
            Vector3 spawnPos = stackOrigin.position + stackDirection.normalized * stackSpacing * stackedPrefabs.Count + Vector3.up * stackYOffset;
            Quaternion spawnRot = Quaternion.LookRotation(stackDirection.normalized);
            stacked.transform.position = spawnPos;
            stacked.transform.rotation = spawnRot;
            stacked.transform.localScale = stackedScale;
            // Desativa física e colisão real
            var rb = stacked.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true;
            var colliders = stacked.GetComponentsInChildren<Collider>();
            foreach (var col in colliders) col.isTrigger = true;
            stackedPrefabs.Add(stacked);
        }
        else
        {
            Debug.LogWarning("[StackManager] stackedEnemyPrefab não atribuído!");
        }
        // Desativa ou destrói o inimigo original
        enemy.gameObject.SetActive(false);
    }

    public int StackCount => stackedPrefabs.Count;
} 