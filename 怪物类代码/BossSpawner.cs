using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public Transform bossSpawnPoint; // Boss ������λ��
    public GameObject bossPrefab;     // ��Ҫ���ɵ� Boss Ԥ����
    private const string BossSpawnedKey = "BossSpawned"; // ���� PlayerPrefs �ļ�

    private bool bossSpawned = false; // ��� Boss �Ƿ��Ѿ�����

    void Start()
    {
        // �� PlayerPrefs �ж�ȡ Boss �Ƿ��Ѿ����ֹ��ı�־
        bossSpawned = PlayerPrefs.GetInt(BossSpawnedKey, 0) == 1;
    }

    void Update()
    {
        // ��鳡�����Ƿ��д��С�enemy����ǩ�Ĺ���
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        Debug.Log($"Enemy count: {enemies.Length}"); // �������

        // ���û�е����� Boss ��û�г���
        if (enemies.Length == 0 && !bossSpawned)
        {
            SpawnBoss();
        }
    }

    void SpawnBoss()
    {
        if (bossPrefab != null && bossSpawnPoint != null)
        {
            // ʵ���� Boss Ԥ����
            GameObject bossInstance = Instantiate(bossPrefab, bossSpawnPoint.position, bossSpawnPoint.rotation);

            // ������ʵ���� BossSpawner �ű�
            BossSpawner newBossSpawner = bossInstance.GetComponent<BossSpawner>();
            if (newBossSpawner != null)
            {
                newBossSpawner.enabled = false;
            }

            bossSpawned = true; // ��� Boss �Ѿ�����
            // �� Boss �Ѿ����ֹ��ı�־���浽 PlayerPrefs ��
            PlayerPrefs.SetInt(BossSpawnedKey, 1);
            PlayerPrefs.Save(); // ȷ�����ݱ���

            Debug.Log("Boss has spawned!");
        }
        else
        {
            Debug.LogError("Boss prefab or spawn point is not assigned!");
        }
    }
}