using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FightingSceneLoader : MonoBehaviour
{
    public static Dictionary<ItemData, int> backpackItemsBeforeSwitch;
    public Camera mainCamera;
    public MonsterData[] monsterDataList;
    public static Dictionary<ItemData, int> backpackStateBeforeBattle;

    // ���� NPCDialogue �ű�
    public NPCDialogue npcDialogue;

    private void Start()
    {
        // �������ս��ǰ�ı���״̬
        backpackStateBeforeBattle = BackpackManager.Instance.GetAllItemsInBackpack();
        Debug.Log($"����ս��ǰ�ı�����Ʒ������{backpackStateBeforeBattle.Count}");

        // ��� BackpackManager �Ƿ��Ѿ���ʼ��
        if (BackpackManager.Instance == null)
        {
            Debug.LogError("BackpackManager δ�ҵ���");
            return;
        }

        // ��ȡ��ǰ�����е���Ʒ��Ϣ
        backpackItemsBeforeSwitch = BackpackManager.Instance.GetAllItemsInBackpack();
        Debug.Log($"��ǰ������Ʒ������{backpackItemsBeforeSwitch.Count}");

        // ��¼�Ի�״̬�� PlayerPrefs
        if (npcDialogue != null)
        {
            PlayerPrefs.SetInt("IsFirstDialogueCompleted", npcDialogue.isFirstDialogueCompleted ? 1 : 0);
            PlayerPrefs.SetInt("IsSecondDialogueCompleted", npcDialogue.isSecondDialogueCompleted ? 1 : 0);
            PlayerPrefs.SetInt("IsDoingSpecialDialogue", npcDialogue.isSpecialDialogueCompleted ? 1 : 0);
            PlayerPrefs.SetInt("IsDoingFinalDialogue", npcDialogue.isFinalDialogueCompleted ? 1 : 0);
            PlayerPrefs.Save(); // ȷ�����ݱ���
        }

        // ��ȡ֮ǰ������С��������
        string enemyName = PlayerPrefs.GetString("EnemyName");

        // ����Ƿ������˵�������
        if (string.IsNullOrEmpty(enemyName))
        {
            Debug.LogError("EnemyName is not set in PlayerPrefs!");
            return;
        }

        // ȥ�� (Clone) ��׺
        if (enemyName.Contains("(Clone)"))
        {
            enemyName = enemyName.Replace("(Clone)", "").Trim();
        }

        // ���Ҷ�Ӧ�Ĺ�������
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

        Debug.Log($"�ҵ��������ݣ�{monsterData.monsterName}��ƥ��ĵ������ƣ�{enemyName}");

        // ʵ��������
        GameObject instantiatedEnemy = Instantiate(monsterData.prefab, transform.position, Quaternion.identity);
        Debug.Log($"����ʵ�����ɹ���{instantiatedEnemy.name}");

        // ������ŵ����߼�
        if (monsterData.monsterName == "Black")
        {
            instantiatedEnemy.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            Debug.Log("Black ������������Ϊ 0.4");
        }

        if (mainCamera != null)
        {
            Debug.Log("���������ȷ����");
            Vector3 cameraForward = mainCamera.transform.forward;
            cameraForward.y = 0;  // ȷ��ֻ��ˮƽ������ת
            cameraForward.Normalize();

            instantiatedEnemy.transform.forward = -cameraForward;
        }
        else
        {
            Debug.LogError("Main camera is not assigned!");
        }

        // �������ݸ�ս��������
        BattleMechanismManager battleManager = FindObjectOfType<BattleMechanismManager>();
        if (battleManager != null)
        {
            Debug.Log("ս�����������ҵ�");
            Debug.Log($"���ݵĵ������� enemyName Ϊ��{enemyName}");
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