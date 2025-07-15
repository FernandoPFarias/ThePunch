// StackManager: Gerencia a pilha de inimigos coletados pelo player, efeito de inércia, coleta e venda.
public class StackManager : MonoBehaviour
{
    public Transform stackOrigin; // Ponto de origem da pilha nas costas do player
    public float stackSpacing = 1.2f; // Espaço vertical entre inimigos
    public float followSpeed = 10f; // Velocidade de suavização/inércia
    public float pickupRange = 2.5f; // Range para pegar inimigos derrotados
    public LayerMask enemyLayer;
    // ... existing code ...
    void Start()
    {
        // Inicializa UI e corrotina de checagem de inimigos
        UpdateCapacityBar();
        if (OnStackChanged != null)
            OnStackChanged += _ => { UpdateCapacityBar(); };
        else
            OnStackChanged = _ => { UpdateCapacityBar(); };
        StartCoroutine(CheckEnemiesRoutine());
        lastStackOriginPosition = stackOrigin.position;
        // Inicializa velocities para SmoothDamp
        velocities = new List<Vector3>();
        for (int i = 0; i < 32; i++) // Suporta até 32 inimigos empilhados
            velocities.Add(Vector3.zero);
    }

    // Corrotina que checa periodicamente inimigos derrotados próximos para coletar
    private IEnumerator CheckEnemiesRoutine()
    {
        while (true)
        {
            Collider[] hits = Physics.OverlapSphere(stackOrigin.position, pickupRange, enemyLayer);
            foreach (var hit in hits)
            {
                var stackable = hit.GetComponentInParent<StackableCharacter>();
                var enemyController = hit.GetComponentInParent<EnemyController>();
                // Só coleta inimigos derrotados, liberados e não empilhados
                if (stackable != null && enemyController != null && enemyController.IsRagdollActive() && !stackable.isStacked && stackable.canBeCollected)
                {
                    if (stackedPrefabs.Count < maxStack)
                    {
                        AddToStack(stackable.transform);
                        stackable.isStacked = true;
                        stackable.canBeCollected = false;
                    }
                }
            }
            yield return new WaitForSeconds(enemyCheckInterval);
        }
    }

    void Update()
    {
        // Atualiza posições da pilha a cada frame para seguir o player (efeito de inércia)
        Vector3 dir = stackDirection.normalized;
        Vector3 anchor = stackOrigin.position;
        if (stackedPrefabs.Count > 0)
        {
            // O primeiro empilhado segue diretamente o ponto de origem
            stackedPrefabs[0].transform.position = stackOrigin.position + dir * stackSpacing + Vector3.up * stackYOffset;
            stackedPrefabs[0].transform.rotation = Quaternion.Euler(90, 0, 0);
            stackedPrefabs[0].transform.localScale = stackedScale;
        }
        // Os demais seguem o anterior com inércia (SmoothDamp)
        for (int i = 1; i < stackedPrefabs.Count; i++)
        {
            float t = (float)i / (stackedPrefabs.Count - 1);
            float smooth = 1f / Mathf.Lerp(lerpSpeed, lerpSpeed * 2f, t); // Quanto maior lerpSpeed, mais rápido
            Vector3 targetPos = stackedPrefabs[i - 1].transform.position + dir * stackSpacing + Vector3.up * stackYOffset;
            Vector3 vel = velocities[i];
            stackedPrefabs[i].transform.position = Vector3.SmoothDamp(
                stackedPrefabs[i].transform.position,
                targetPos,
                ref vel,
                smooth
            );
            velocities[i] = vel;
            stackedPrefabs[i].transform.rotation = Quaternion.Euler(90, 0, 0);
            stackedPrefabs[i].transform.localScale = stackedScale;
        }
    }

    // Atualiza a barra de capacidade visual
    public void UpdateCapacityBar()
    {
        if (capacityBarUI != null)
        {
            capacityBarUI.SetCapacity(maxStack, maxPossibleCapacity);
            capacityBarUI.SetLoad(stackedPrefabs.Count);
        }
    }

    // Adiciona um inimigo à pilha visual e desativa o original
    public void AddToStack(Transform enemy)
    {
        if (stackedPrefabs.Count >= maxStack)
            return;
        // Usa o pool para pegar o visual da pilha
        GameObject stacked = null;
        if (stackPool != null)
        {
            stacked = stackPool.GetStackedEnemy();
        }
        else if (stackedEnemyPrefab != null)
        {
            stacked = Instantiate(stackedEnemyPrefab);
        }
        else
        {
            Debug.LogWarning("[StackManager] stackedEnemyPrefab não atribuído!");
        }
        if (stacked != null)
        {
            stacked.transform.position = Vector3.zero;
            stacked.transform.rotation = Quaternion.identity;
            stacked.transform.localScale = Vector3.one;
            Vector3 spawnPos = stackOrigin.position + stackDirection.normalized * stackSpacing * stackedPrefabs.Count + Vector3.up * stackYOffset;
            Quaternion spawnRot = Quaternion.LookRotation(stackDirection.normalized);
            stacked.transform.position = spawnPos;
            stacked.transform.rotation = spawnRot;
            stacked.transform.localScale = stackedScale;
            var rb = stacked.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true;
            var colliders = stacked.GetComponentsInChildren<Collider>();
            foreach (var col in colliders) col.isTrigger = true;
            stackedPrefabs.Add(stacked);
            // Garante que velocities tenha tamanho suficiente
            while (velocities.Count < stackedPrefabs.Count)
                velocities.Add(Vector3.zero);
        }
        // Desativa ou destrói o inimigo original
        var enemyController = enemy.GetComponent<EnemyController>();
        if (enemyController != null) {
            enemyController.DisablePhysicsAndColliders();
            enemyController.RemoveEnemy();
        } else {
            Destroy(enemy.gameObject);
        }
        OnStackChanged?.Invoke(stackedPrefabs.Count);
        UpdateCapacityBar();
    }

    // Vende todos os empilhados, limpa a pilha e atualiza dinheiro/UI
    public void SellStack()
    {
        int sold = stackedPrefabs.Count;
        foreach (var obj in stackedPrefabs)
        {
            if (stackPool != null)
                stackPool.ReturnStackedEnemy(obj);
            else
                Destroy(obj);
        }
        stackedPrefabs.Clear();
        // Limpa velocities para evitar lixo
        velocities.Clear();
        money += sold * moneyPerEnemy;
        OnMoneyChanged?.Invoke(money);
        OnStackChanged?.Invoke(stackedPrefabs.Count);
        UpdateCapacityBar();
    }
// ... existing code ... 