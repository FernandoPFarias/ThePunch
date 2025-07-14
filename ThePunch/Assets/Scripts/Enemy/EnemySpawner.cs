using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Vector3 areaCenter;
    public Vector3 areaSize = new Vector3(10, 0, 10);
    public float spawnInterval = 3f;
    public int maxEnemies = 10;

    private float timer = 0f;
    private int currentEnemies = 0;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval && currentEnemies < maxEnemies)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        Vector3 randomPos = areaCenter + new Vector3(
            Random.Range(-areaSize.x / 2, areaSize.x / 2),
            0,
            Random.Range(-areaSize.z / 2, areaSize.z / 2)
        );
        GameObject enemy = Instantiate(enemyPrefab, randomPos, Quaternion.identity);
        currentEnemies++;
        var controller = enemy.GetComponent<EnemyController>();
        if (controller != null)
            controller.OnEnemyRemoved += OnEnemyRemoved;
    }

    void OnEnemyRemoved()
    {
        currentEnemies--;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.2f);
        Gizmos.DrawCube(areaCenter, areaSize);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(areaCenter, areaSize);
    }
} 