// ===============================
// StackPool.cs
// Gerencia o pool de objetos empilhados (stacked enemies) para otimizar performance.
// ===============================
using System.Collections.Generic;
using UnityEngine;

public class StackPool : MonoBehaviour
{
    [Header("Configuração do Pool")]
    [Tooltip("Prefab do inimigo empilhado")] public GameObject stackedEnemyPrefab;
    [Tooltip("Tamanho inicial do pool")] public int poolSize = 10;
    private List<GameObject> pool = new List<GameObject>();

    void Awake()
    {
        // Inicializa o pool com objetos desativados
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(stackedEnemyPrefab);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public GameObject GetStackedEnemy()
    {
        // Retorna um objeto inativo do pool ou instancia um novo se necessário
        foreach (var obj in pool)
        {
            if (obj != null && !obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }
        GameObject objNew = Instantiate(stackedEnemyPrefab);
        pool.Add(objNew);
        return objNew;
    }

    public void ReturnStackedEnemy(GameObject obj)
    {
        // Desativa o objeto para reaproveitamento
        obj.SetActive(false);
    }
} 