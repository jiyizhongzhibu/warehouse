using UnityEngine;
using TMPro;

public class TaskPanelUI : MonoBehaviour
{
    public GameObject taskPanel;
    public TextMeshProUGUI taskText;
    private NPCDialogue npcDialogue;

    // 用于标记各种对话完成状态是否已经处理过
    private bool firstDialogueProcessed = false;
    private bool secondDialogueProcessed = false;
    private bool specialDialogueProcessed = false;
    private bool finalDialogueProcessed = false;
    private bool endingDialogueProcessed = false;

    void Start()
    {
        // 初始化时隐藏任务面板
        taskPanel.SetActive(false);

        // 获取 NPCDialogue 脚本的引用
        npcDialogue = FindObjectOfType<NPCDialogue>();
        if (npcDialogue == null)
        {
            Debug.LogError("NPCDialogue 脚本未找到！");
        }
    }

    void Update()
    {
        // 根据对话完成状态更新任务面板内容
        UpdateTaskPanel();
    }

    private void UpdateTaskPanel()
    {
        if (npcDialogue == null) return;

        if (npcDialogue.isFirstDialogueCompleted && !firstDialogueProcessed)
        {
            // 第一次对话完成后显示任务面板
            taskPanel.SetActive(true);
            taskText.text = "跟随爱莎打败怪物。";
            firstDialogueProcessed = true; // 标记为已处理
        }

        if (npcDialogue.isSecondDialogueCompleted && !secondDialogueProcessed)
        {
            taskPanel.SetActive(true);
            taskText.text = "请接触怪物打败他！";
            secondDialogueProcessed = true; // 标记为已处理
        }

        if (npcDialogue.isSpecialDialogueCompleted && !specialDialogueProcessed)
        {
            // 计算剩余怪物数量
            int remainingEnemies = GameObject.FindGameObjectsWithTag("enemy").Length;
            taskText.text = $"请找到所有怪物并击败他们，剩余怪物为 {remainingEnemies} 个。";
            taskPanel.SetActive(true);
            specialDialogueProcessed = true; // 标记为已处理
        }

        if (npcDialogue.isFinalDialogueCompleted && !finalDialogueProcessed)
        {
            taskPanel.SetActive(true);
            taskText.text = "请跟随爱莎击败魔王，他正隐藏在地图的最深处。";
            finalDialogueProcessed = true; // 标记为已处理
        }

        if (npcDialogue.isEndingDialogueCompleted && !endingDialogueProcessed)
        {
            taskPanel.SetActive(true);
            taskText.text = "恭喜您击败了所有敌人，拯救了双语王国";
            endingDialogueProcessed = true; // 标记为已处理
        }
    }
}