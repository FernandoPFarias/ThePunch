using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Rigidbody[] ragdollBodies;
    public Animator animator;
    public Collider[] ragdollColliders;
    public Transform hips; // arraste o osso principal do ragdoll aqui
    public CharacterController characterController;
    public UnityEngine.AI.NavMeshAgent navMeshAgent;
    public Rigidbody rootRigidbody;
    public SimplePatrol simplePatrol;

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
        SetRagdoll(false);
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
        if (animator != null)
            animator.enabled = !active;
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
    }

    public void ActivateRagdoll(GameObject player = null)
    {
        SetRagdoll(true);
        if (player != null)
            StartCoroutine(IgnorePlayerCollision(player, 1.5f)); // ignora por 1.5 segundos
    }

    private IEnumerator IgnorePlayerCollision(GameObject player, float duration)
    {
        var playerColliders = player.GetComponentsInChildren<Collider>();
        foreach (var ragdollCol in ragdollColliders)
        {
            foreach (var playerCol in playerColliders)
            {
                Physics.IgnoreCollision(ragdollCol, playerCol, true);
            }
        }
        yield return new WaitForSeconds(duration);
        foreach (var ragdollCol in ragdollColliders)
        {
            foreach (var playerCol in playerColliders)
            {
                Physics.IgnoreCollision(ragdollCol, playerCol, false);
            }
        }
    }

    void LateUpdate()
    {
        if (hips != null && animator != null && !animator.enabled)
        {
            // Sincroniza o root com o hips enquanto o ragdoll está ativo
            transform.position = hips.position;
        }
    }
} 