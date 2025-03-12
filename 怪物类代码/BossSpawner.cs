using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public Transform bossSpawnPoint; // Boss 出场的位置
    public GameObject bossPrefab;     // 需要生成的 Boss 预制体
    private const string BossSpawnedKey = "BossSpawned"; // 用于 PlayerPrefs 的键

    private bool bossSpawned = false; // 标记 Boss 是否已经出场

    void Start()
    {
        // 从 PlayerPrefs 中读取 Boss 是否已经出现过的标志
        bossSpawned = PlayerPrefs.GetInt(BossSpawnedKey, 0) == 1;
    }

    void Update()
    {
        // 检查场景中是否还有带有“enemy”标签的怪物
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        Debug.Log($"Enemy count: {enemies.Length}"); // 调试输出

        // 如果没有敌人且 Boss 还没有出场
        if (enemies.Length == 0 && !bossSpawned)
        {
            SpawnBoss();
        }
    }

    void SpawnBoss()
    {
        if (bossPrefab != null && bossSpawnPoint != null)
        {
            // 实例化 Boss 预制体
            GameObject bossInstance = Instantiate(bossPrefab, bossSpawnPoint.position, bossSpawnPoint.rotation);

            // 禁用新实例的 BossSpawner 脚本
            BossSpawner newBossSpawner = bossInstance.GetComponent<BossSpawner>();
            if (newBossSpawner != null)
            {
                newBossSpawner.enabled = false;
            }

            bossSpawned = true; // 标记 Boss 已经出场
            // 将 Boss 已经出现过的标志保存到 PlayerPrefs 中
            PlayerPrefs.SetInt(BossSpawnedKey, 1);
            PlayerPrefs.Save(); // 确保数据保存

            Debug.Log("Boss has spawned!");
        }
        else
        {
            Debug.LogError("Boss prefab or spawn point is not assigned!");
        }
    }
}