// ===============================
// EnemySpawner.cs
// Responsável por spawnar inimigos em uma área definida, usando pooling se disponível.
// Exponha os campos necessários no Inspector com headers e tooltips para facilitar a configuração.
// ===============================
using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("Configuração de Spawn")]
    [Tooltip("Prefab do inimigo a ser spawnado")] public GameObject enemyPrefab;
    [Tooltip("Centro da área de spawn")] public Vector3 areaCenter;
    [Tooltip("Tamanho da área de spawn")] public Vector3 areaSize = new Vector3(10, 0, 10);
    [Tooltip("Intervalo entre spawns (segundos)")] public float spawnInterval = 3f;
    [Tooltip("Número máximo de inimigos na área")] public int maxEnemies = 10;
    [Header("Pooling")]
    [Tooltip("Pool de inimigos para reaproveitamento")] public EnemyPool enemyPool;

    private int currentEnemies = 0;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            if (currentEnemies < maxEnemies)
            {
                SpawnEnemy();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy()
    {
        Vector3 randomPos = areaCenter + new Vector3(
            Random.Range(-areaSize.x / 2, areaSize.x / 2),
            0,
            Random.Range(-areaSize.z / 2, areaSize.z / 2)
        );
        GameObject enemy = enemyPool != null ? enemyPool.GetEnemy() : Instantiate(enemyPrefab);
        enemy.transform.position = randomPos;
        enemy.transform.rotation = Quaternion.identity;
        currentEnemies++;
        var controller = enemy.GetComponent<EnemyController>();
        if (controller != null)
            controller.OnEnemyRemoved += OnEnemyRemoved;
    }

    void OnEnemyRemoved()
    {
        currentEnemies--;
        Debug.Log($"[EnemySpawner] OnEnemyRemoved chamado. currentEnemies: {currentEnemies}");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.2f);
        Gizmos.DrawCube(areaCenter, areaSize);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(areaCenter, areaSize);
    }
} 