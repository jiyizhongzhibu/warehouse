using UnityEngine;
using UnityEngine.SceneManagement;

public class StartLoader : MonoBehaviour
{
    private const string BossSpawnedKey = "BossSpawned";

    public void SwitchToScene1()
    {
        // 初始化所有对话标志
        PlayerPrefs.SetInt("IsFirstDialogueCompleted", 0);
        PlayerPrefs.SetInt("IsSecondDialogueCompleted", 0);
        PlayerPrefs.SetInt("IsSpecialDialogueCompleted", 0);
        PlayerPrefs.SetInt("IsFinalDialogueCompleted", 0);
        PlayerPrefs.SetInt("IsEndingDialogueCompleted", 0);

        // 清空敌人和道具的持久化记录
        PlayerPrefs.SetString("DefeatedEnemies", "");
        PlayerPrefs.SetString("DestroyedModels", "");  // 新增关键代码

        // 重置Boss状态
        PlayerPrefs.SetInt(BossSpawnedKey, 0);

        // 强制保存所有修改
        PlayerPrefs.Save();
        Debug.Log("【场景切换】已初始化所有状态：\n" +
                 "- 对话标志重置\n" +
                 "- 敌人/道具记录清空\n" +
                 "- Boss未生成");

        // 加载场景
        SceneManager.LoadScene("1");
    }
}