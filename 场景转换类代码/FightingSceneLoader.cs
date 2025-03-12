using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FightingSceneLoader : MonoBehaviour
{
    public static Dictionary<ItemData, int> backpackItemsBeforeSwitch;
    public Camera mainCamera;
    public MonsterData[] monsterDataList;
    public static Dictionary<ItemData, int> backpackStateBeforeBattle;

    // 引用 NPCDialogue 脚本
    public NPCDialogue npcDialogue;

    private void Start()
    {
        // 保存进入战斗前的背包状态
        backpackStateBeforeBattle = BackpackManager.Instance.GetAllItemsInBackpack();
        Debug.Log($"进入战斗前的背包物品数量：{backpackStateBeforeBattle.Count}");

        // 检查 BackpackManager 是否已经初始化
        if (BackpackManager.Instance == null)
        {
            Debug.LogError("BackpackManager 未找到！");
            return;
        }

        // 获取当前背包中的物品信息
        backpackItemsBeforeSwitch = BackpackManager.Instance.GetAllItemsInBackpack();
        Debug.Log($"当前背包物品数量：{backpackItemsBeforeSwitch.Count}");

        // 记录对话状态到 PlayerPrefs
        if (npcDialogue != null)
        {
            PlayerPrefs.SetInt("IsFirstDialogueCompleted", npcDialogue.isFirstDialogueCompleted ? 1 : 0);
            PlayerPrefs.SetInt("IsSecondDialogueCompleted", npcDialogue.isSecondDialogueCompleted ? 1 : 0);
            PlayerPrefs.SetInt("IsDoingSpecialDialogue", npcDialogue.isSpecialDialogueCompleted ? 1 : 0);
            PlayerPrefs.SetInt("IsDoingFinalDialogue", npcDialogue.isFinalDialogueCompleted ? 1 : 0);
            PlayerPrefs.Save(); // 确保数据保存
        }

        // 获取之前碰到的小怪物名称
        string enemyName = PlayerPrefs.GetString("EnemyName");

        // 检查是否设置了敌人名称
        if (string.IsNullOrEmpty(enemyName))
        {
            Debug.LogError("EnemyName is not set in PlayerPrefs!");
            return;
        }

        // 去除 (Clone) 后缀
        if (enemyName.Contains("(Clone)"))
        {
            enemyName = enemyName.Replace("(Clone)", "").Trim();
        }

        // 查找对应的怪物数据
        MonsterData monsterData = null;
        foreach (var data in monsterDataList)
        {
            if (data.monsterName == enemyName)
            {
                monsterData = data;
                break;
            }
        }

        if (monsterData == null)
        {
            Debug.LogError($"No matching monster data found for name: {enemyName}");
            return;
        }

        Debug.Log($"找到怪物数据：{monsterData.monsterName}，匹配的敌人名称：{enemyName}");

        // 实例化怪物
        GameObject instantiatedEnemy = Instantiate(monsterData.prefab, transform.position, Quaternion.identity);
        Debug.Log($"怪物实例化成功：{instantiatedEnemy.name}");

        // 添加缩放调整逻辑
        if (monsterData.monsterName == "Black")
        {
            instantiatedEnemy.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            Debug.Log("Black 怪物缩放设置为 0.4");
        }

        if (mainCamera != null)
        {
            Debug.Log("主相机已正确设置");
            Vector3 cameraForward = mainCamera.transform.forward;
            cameraForward.y = 0;  // 确保只在水平方向旋转
            cameraForward.Normalize();

            instantiatedEnemy.transform.forward = -cameraForward;
        }
        else
        {
            Debug.LogError("Main camera is not assigned!");
        }

        // 传递数据给战斗管理器
        BattleMechanismManager battleManager = FindObjectOfType<BattleMechanismManager>();
        if (battleManager != null)
        {
            Debug.Log("战斗管理器已找到");
            Debug.Log($"传递的敌人名称 enemyName 为：{enemyName}");
            battleManager.monsterData = monsterData;
            battleManager.SetCurrentEnemyName(enemyName);
            battleManager.InitializeMonster();
        }
        else
        {
            Debug.LogError("BattleMechanismManager not found in scene!");
        }
    }
}