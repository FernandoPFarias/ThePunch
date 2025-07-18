// ===============================
// EnemyController.cs
// Responsável por controlar o inimigo, ativar/desativar ragdoll, pooling, colisão e eventos de remoção.
// Exponha os campos necessários no Inspector com headers e tooltips para facilitar a configuração.
// ===============================
using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Referências de Componentes")]
    [Tooltip("Rigidbodies do ragdoll do inimigo")] public Rigidbody[] ragdollBodies;
    [Tooltip("Animator do inimigo")] public Animator animator;
    [Tooltip("Colliders do ragdoll do inimigo")] public Collider[] ragdollColliders;
    [Tooltip("Transform do osso principal do ragdoll (hips)")] public Transform hips;
    [Tooltip("CharacterController do inimigo")] public CharacterController characterController;
    [Tooltip("NavMeshAgent do inimigo")] public UnityEngine.AI.NavMeshAgent navMeshAgent;
    [Tooltip("Rigidbody raiz do inimigo")] public Rigidbody rootRigidbody;
    [Tooltip("Script de patrulha simples (opcional)")] public SimplePatrol simplePatrol;
    private int originalLayer;
    private const string ragdollLayerName = "RagdollDead";
    public System.Action OnEnemyRemoved;
    [Header("Pooling e Eventos")]
    [Tooltip("Pool de inimigos para reaproveitamento")] public EnemyPool enemyPool;

    void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        if (ragdollBodies == null || ragdollBodies.Length == 0)
            ragdollBodies = GetComponentsInChildren<Rigidbody>();
        if (ragdollColliders == null || ragdollColliders.Length == 0)
            ragdollColliders = GetComponentsInChildren<Collider>();
        if (characterController == null)
            characterController = GetComponent<CharacterController>();
        if (navMeshAgent == null)
            navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (rootRigidbody == null)
            rootRigidbody = GetComponent<Rigidbody>();
        if (simplePatrol == null)
            simplePatrol = GetComponent<SimplePatrol>();
        // NÃO desative o Character Controller aqui!
        // Mantenha o Character Controller ativo até ativar o ragdoll
        // SetRagdoll(false) agora só desativa ragdollColliders
        originalLayer = gameObject.layer;
        SetRagdoll(false);
        if (characterController != null)
            characterController.enabled = true;
    }

    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    public void SetRagdoll(bool active)
    {
        foreach (var rb in ragdollBodies)
        {
            rb.isKinematic = !active;
            rb.detectCollisions = active;
            if (active)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.constraints = RigidbodyConstraints.None;
            }
            else
            {
                rb.constraints = RigidbodyConstraints.None;
            }
        }
        // Garante que os colliders do ragdoll estejam habilitados e triggers ao ativar
        foreach (var col in ragdollColliders)
        {
            col.enabled = active;
            // Removido: alteração de isTrigger
        }
        if (animator != null)
            animator.enabled = !active;
        // Layer logic
        if (active)
        {
            int ragdollLayer = LayerMask.NameToLayer(ragdollLayerName);
            if (ragdollLayer >= 0)
                SetLayerRecursively(gameObject, ragdollLayer);
        }
        else
        {
            SetLayerRecursively(gameObject, originalLayer);
        }
        // Desativa movimentação do root ao ativar ragdoll
        if (active)
        {
            if (characterController != null)
                characterController.enabled = false;
            if (navMeshAgent != null)
                navMeshAgent.enabled = false;
            if (rootRigidbody != null)
                rootRigidbody.isKinematic = true;
            if (simplePatrol != null)
                simplePatrol.SetActive(false);
        }
        else
        {
            // Ao desativar ragdoll, reativa o Character Controller
            if (characterController != null)
                characterController.enabled = true;
        }
    }

    public void ActivateRagdoll(GameObject player = null)
    {
        if (player != null)
            IgnorePlayerCollisionImmediate(player, true); // Ignora colisão antes de ativar ragdoll
        if (characterController != null)
            characterController.enabled = false;
        StartCoroutine(ActivateRagdollDelayed(player));
        // Permite coleta após delay
        var stackable = GetComponent<StackableCharacter>();
        if (stackable != null)
            StartCoroutine(EnableCollectAfterDelay(stackable));
    }

    private IEnumerator ActivateRagdollDelayed(GameObject player)
    {
        yield return new WaitForFixedUpdate(); // Espera um frame de física
        SetRagdoll(true);
        if (player != null)
            StartCoroutine(RestorePlayerCollisionAfterDelay(player, 1.5f));
    }

    private IEnumerator EnableCollectAfterDelay(StackableCharacter stackable)
    {
        stackable.canBeCollected = false;
        yield return new WaitForSeconds(stackable.collectDelay);
        stackable.canBeCollected = true;
    }

    // Ignora colisão imediatamente
    private void IgnorePlayerCollisionImmediate(GameObject player, bool ignore)
    {
        var playerColliders = player.GetComponentsInChildren<Collider>();
        foreach (var ragdollCol in ragdollColliders)
        {
            foreach (var playerCol in playerColliders)
            {
                Physics.IgnoreCollision(ragdollCol, playerCol, ignore);
            }
        }
    }

    // Restaura colisão após um tempo
    private IEnumerator RestorePlayerCollisionAfterDelay(GameObject player, float duration)
    {
        yield return new WaitForSeconds(duration);
        IgnorePlayerCollisionImmediate(player, false);
    }

    public bool IsRagdollActive()
    {
        return animator != null && !animator.enabled;
    }

    public void RemoveEnemy()
    {
        Debug.Log($"[EnemyController] RemoveEnemy chamado para {gameObject.name}. Pool: {(enemyPool != null)}");
        OnEnemyRemoved?.Invoke();
        if (enemyPool != null)
            enemyPool.ReturnEnemy(gameObject);
        else
            Destroy(gameObject);
    }

    public void DisablePhysicsAndColliders()
    {
        // Desativa todos os rigidbodies e colliders do inimigo
        if (ragdollBodies != null)
        {
            foreach (var rb in ragdollBodies)
            {
                if (rb != null) rb.isKinematic = true;
            }
        }
        if (ragdollColliders != null)
        {
            foreach (var col in ragdollColliders)
            {
                if (col != null) col.enabled = false;
            }
        }
        if (characterController != null) characterController.enabled = false;
        if (navMeshAgent != null) navMeshAgent.enabled = false;
    }

    void LateUpdate()
    {
        if (hips != null && animator != null && !animator.enabled)
        {
            // Sincroniza o root com o hips enquanto o ragdoll está ativo
            transform.position = hips.position;
            transform.rotation = hips.rotation;
        }
    }
} 