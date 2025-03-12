using UnityEngine;
using TMPro;

public class TaskPanelUI : MonoBehaviour
{
    public GameObject taskPanel;
    public TextMeshProUGUI taskText;
    private NPCDialogue npcDialogue;

    // ���ڱ�Ǹ��ֶԻ����״̬�Ƿ��Ѿ������
    private bool firstDialogueProcessed = false;
    private bool secondDialogueProcessed = false;
    private bool specialDialogueProcessed = false;
    private bool finalDialogueProcessed = false;
    private bool endingDialogueProcessed = false;

    void Start()
    {
        // ��ʼ��ʱ�����������
        taskPanel.SetActive(false);

        // ��ȡ NPCDialogue �ű�������
        npcDialogue = FindObjectOfType<NPCDialogue>();
        if (npcDialogue == null)
        {
            Debug.LogError("NPCDialogue �ű�δ�ҵ���");
        }
    }

    void Update()
    {
        // ���ݶԻ����״̬���������������
        UpdateTaskPanel();
    }

    private void UpdateTaskPanel()
    {
        if (npcDialogue == null) return;

        if (npcDialogue.isFirstDialogueCompleted && !firstDialogueProcessed)
        {
            // ��һ�ζԻ���ɺ���ʾ�������
            taskPanel.SetActive(true);
            taskText.text = "���氮ɯ��ܹ��";
            firstDialogueProcessed = true; // ���Ϊ�Ѵ���
        }

        if (npcDialogue.isSecondDialogueCompleted && !secondDialogueProcessed)
        {
            taskPanel.SetActive(true);
            taskText.text = "��Ӵ�����������";
            secondDialogueProcessed = true; // ���Ϊ�Ѵ���
        }

        if (npcDialogue.isSpecialDialogueCompleted && !specialDialogueProcessed)
        {
            // ����ʣ���������
            int remainingEnemies = GameObject.FindGameObjectsWithTag("enemy").Length;
            taskText.text = $"���ҵ����й��ﲢ�������ǣ�ʣ�����Ϊ {remainingEnemies} ����";
            taskPanel.SetActive(true);
            specialDialogueProcessed = true; // ���Ϊ�Ѵ���
        }

        if (npcDialogue.isFinalDialogueCompleted && !finalDialogueProcessed)
        {
            taskPanel.SetActive(true);
            taskText.text = "����氮ɯ����ħ�������������ڵ�ͼ�������";
            finalDialogueProcessed = true; // ���Ϊ�Ѵ���
        }

        if (npcDialogue.isEndingDialogueCompleted && !endingDialogueProcessed)
        {
            taskPanel.SetActive(true);
            taskText.text = "��ϲ�����������е��ˣ�������˫������";
            endingDialogueProcessed = true; // ���Ϊ�Ѵ���
        }
    }
}