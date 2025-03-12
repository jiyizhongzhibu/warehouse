using UnityEngine;

public class Scene1Loader : MonoBehaviour
{
    private void Start()
    {
        // 检查战斗胜利标记
        if (PlayerPrefs.GetInt("BattleWon") == 1)
        {
            // 恢复玩家位置
            float playerX = PlayerPrefs.GetFloat("PlayerX");
            float playerY = PlayerPrefs.GetFloat("PlayerY");
            float playerZ = PlayerPrefs.GetFloat("PlayerZ");
            transform.position = new Vector3(playerX, playerY, playerZ);

            // 重置战斗胜利标记
            PlayerPrefs.SetInt("BattleWon", 0);

            // 处理已击败的敌人
            string defeatedEnemies = PlayerPrefs.GetString("DefeatedEnemies", "");
            Debug.Log($"Scene1Loader 处理的 DefeatedEnemies: {defeatedEnemies}");

            string[] enemyNames = defeatedEnemies.Split(',');
            foreach (string enemyName in enemyNames)
            {
                if (string.IsNullOrEmpty(enemyName)) continue;
                GameObject enemy = GameObject.Find(enemyName);
                if (enemy != null)
                {
                    Debug.Log($"销毁敌人: {enemyName}");
                    Destroy(enemy);
                }
            }

            // 新增：处理已销毁的道具
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
                        Debug.Log($"销毁道具: {modelName}");
                        Destroy(obj);
                    }
                }
            }
        }
    }
}