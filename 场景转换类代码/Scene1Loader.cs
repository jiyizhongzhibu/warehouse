using UnityEngine;

public class Scene1Loader : MonoBehaviour
{
    private void Start()
    {
        // ���ս��ʤ�����
        if (PlayerPrefs.GetInt("BattleWon") == 1)
        {
            // �ָ����λ��
            float playerX = PlayerPrefs.GetFloat("PlayerX");
            float playerY = PlayerPrefs.GetFloat("PlayerY");
            float playerZ = PlayerPrefs.GetFloat("PlayerZ");
            transform.position = new Vector3(playerX, playerY, playerZ);

            // ����ս��ʤ�����
            PlayerPrefs.SetInt("BattleWon", 0);

            // �����ѻ��ܵĵ���
            string defeatedEnemies = PlayerPrefs.GetString("DefeatedEnemies", "");
            Debug.Log($"Scene1Loader ����� DefeatedEnemies: {defeatedEnemies}");

            string[] enemyNames = defeatedEnemies.Split(',');
            foreach (string enemyName in enemyNames)
            {
                if (string.IsNullOrEmpty(enemyName)) continue;
                GameObject enemy = GameObject.Find(enemyName);
                if (enemy != null)
                {
                    Debug.Log($"���ٵ���: {enemyName}");
                    Destroy(enemy);
                }
            }

            // ���������������ٵĵ���
            string destroyedModels = PlayerPrefs.GetString("DestroyedModels", "");
            if (!string.IsNullOrEmpty(destroyedModels))
            {
                string[] modelNames = destroyedModels.Split(',');
                foreach (string modelName in modelNames)
                {
                    if (string.IsNullOrEmpty(modelName)) continue;
                    GameObject obj = GameObject.Find(modelName);
                    if (obj != null)
                    {
                        Debug.Log($"���ٵ���: {modelName}");
                        Destroy(obj);
                    }
                }
            }
        }
    }
}