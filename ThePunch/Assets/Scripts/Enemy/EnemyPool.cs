// ===============================
// EnemyPool.cs
// Responsável por gerenciar o pool de inimigos, evitando instanciamento/destruição frequente.
// Exponha os campos necessários no Inspector com headers e tooltips para facilitar a configuração.
// ===============================
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    [Header("Configuração do Pool")]
    [Tooltip("Prefab do inimigo a ser usado no pool")] public GameObject enemyPrefab;
    [Tooltip("Tamanho inicial do pool")] public int poolSize = 20;
    private List<GameObject> pool = new List<GameObject>();

    void Awake()
    {
        // Inicializa o pool com inimigos desativados
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(enemyPrefab);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public GameObject GetEnemy()
    {
        // Retorna um inimigo inativo do pool ou instancia um novo se necessário
        foreach (var obj in pool)
        {
            if (obj != null && !obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }
        // Se não houver disponível, instancia novo (opcional)
        GameObject objNew = Instantiate(enemyPrefab);
        pool.Add(objNew);
        return objNew;
    }

    public void ReturnEnemy(GameObject obj)
    {
        // Remove assinaturas de eventos e desativa o inimigo para reaproveitamento
        var controller = obj.GetComponent<EnemyController>();
        if (controller != null)
            controller.OnEnemyRemoved = null; // Remove todas as assinaturas
        obj.SetActive(false);
    }
} 