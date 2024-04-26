using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    GameObject prefab;

    [SerializeField]
    int totalEnemiesToSpawn;

    [SerializeField]
    float spawnArea = 1.0f;

    void Start()
    {
        int layerNo = 1;
        for (int i = 0; i < totalEnemiesToSpawn; i++)
        {
            var pos = transform.position + transform.right * Random.Range(-spawnArea * 0.5f, spawnArea * 0.5f);
            
            GameObject enemy = Instantiate(prefab, pos, Quaternion.identity, transform);
            SpriteRenderer enemySpriteRenderer = enemy.GetComponent<SpriteRenderer>();
            
            enemySpriteRenderer.sortingOrder = layerNo;
            layerNo++;
        }
    }

#if UNITY_EDITOR
    [Header("CONTROL PANEL (ONLY EDITOR)")]
    [SerializeField]
    bool showSpawnAreaIfSelected;
    void OnDrawGizmosSelected()
    {
        if (showSpawnAreaIfSelected)
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(spawnArea, 0.4f, 0));
        }
    }
#endif
}
