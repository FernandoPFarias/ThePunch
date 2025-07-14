using UnityEngine;

public class PunchHitbox : MonoBehaviour
{
    public Collider hitboxCollider; // Arraste o collider trigger aqui
    public Color gizmoColor = new Color(1, 0, 0, 0.3f);
    private GameObject lastEnemyHit;
    private GameObject playerRef;

    void Reset()
    {
        hitboxCollider = GetComponent<Collider>();
        if (hitboxCollider != null)
            hitboxCollider.isTrigger = true;
    }

    public bool ActivateHitbox(GameObject player = null)
    {
        if (hitboxCollider != null)
            hitboxCollider.enabled = true;
        playerRef = player;
        lastEnemyHit = null;

        // Checa se há inimigo válido na hitbox
        bool acertou = false;
        Collider[] hits = Physics.OverlapBox(hitboxCollider.bounds.center, hitboxCollider.bounds.extents, hitboxCollider.transform.rotation);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                var enemy = hit.GetComponentInParent<EnemyController>();
                if (enemy != null && !enemy.IsRagdollActive())
                {
                    lastEnemyHit = hit.gameObject;
                    acertou = true;
                    break;
                }
            }
        }
        return acertou;
    }

    public void DeactivateHitbox()
    {
        if (hitboxCollider != null)
            hitboxCollider.enabled = false;
        // Ativa ragdoll do inimigo atingido, se houver
        if (lastEnemyHit != null)
        {
            var enemy = lastEnemyHit.GetComponentInParent<EnemyController>();
            if (enemy != null)
            {
                enemy.ActivateRagdoll(playerRef);
            }
            lastEnemyHit = null;
        }
        playerRef = null;
    }

    void OnDrawGizmos()
    {
        if (hitboxCollider != null && !Application.isPlaying)
        {
            Gizmos.color = gizmoColor;
            if (hitboxCollider is BoxCollider box)
            {
                Gizmos.matrix = box.transform.localToWorldMatrix;
                Gizmos.DrawCube(box.center, box.size);
            }
            else if (hitboxCollider is SphereCollider sphere)
            {
                Gizmos.matrix = sphere.transform.localToWorldMatrix;
                Gizmos.DrawSphere(sphere.center, sphere.radius);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && lastEnemyHit == null)
        {
            lastEnemyHit = other.gameObject;
        }
    }
} 