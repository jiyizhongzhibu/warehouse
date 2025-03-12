using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossDialogue : MonoBehaviour
{
    // �Ի�������Χ
    public float dialogueRange = 5f;
    // ������ʾ�Ի��ı��� UI ���
    public TextMeshProUGUI dialogueText;
    // �Ի����ͼƬ UI ���
    public Image dialogueImage;

    // Boss �Ի���������
    private string[] bossDialogueLines = new string[] {
        "�ߣ����ⲻ֪����ļһ���Ҵ����ҵ���أ�",
        "����������ĩ�գ�û��������ֹ��ͳ����Ƭ���磡",
        "����ǿ���������ǰ���㽫����һ����",
        "׼����ӭ�������Ľ��ٰɣ�"
    };

    // ��ǰ�Ի���������
    private int currentLineIndex = 0;
    // �Ƿ����ڽ��жԻ�
    private bool isInDialogue = false;
    // �Ի��Ƿ��Ѿ�������
    private bool hasDialogueEnded = false;

    void Start()
    {
        // ���ضԻ�����ı�
        HideDialogueUI();
    }

    void Update()
    {
        // ����Ƿ���� Boss ��ǩ�Ķ����ҳ��Դ����Ի�
        if (IsBossPresent())
        {
            TryStartDialogue();
        }

        // �����������Ի�
        HandleDialogueAdvancement();
    }

    // ��鳡�����Ƿ���ڴ��� Boss ��ǩ�Ķ���
    private bool IsBossPresent()
    {
        GameObject[] bosses = GameObject.FindGameObjectsWithTag("Boss");
        return bosses.Length > 0;
    }

    // ���Դ����Ի�
    private void TryStartDialogue()
    {
        if (!isInDialogue && !hasDialogueEnded)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null && Vector3.Distance(transform.position, player.transform.position) < dialogueRange)
            {
                StartDialogue();
            }
        }
    }

    // �����������Ի��߼�
    private void HandleDialogueAdvancement()
    {
        if (isInDialogue && Input.GetMouseButtonDown(0))
        {
            AdvanceDialogue();
        }
    }

    // ��ʼ�Ի�
    private void StartDialogue()
    {
        isInDialogue = true;
        hasDialogueEnded = false;
        currentLineIndex = 0;
        ShowDialogueUI();
        DisplayDialogueLine();
    }

    // ��ʾ��ǰ�Ի���
    private void DisplayDialogueLine()
    {
        if (currentLineIndex < bossDialogueLines.Length)
        {
            dialogueText.text = bossDialogueLines[currentLineIndex];
        }
    }

    // �ƽ��Ի�����ʾ��һ��
    private void AdvanceDialogue()
    {
        currentLineIndex++;
        if (currentLineIndex >= bossDialogueLines.Length)
        {
            EndDialogue();
        }
        else
        {
            DisplayDialogueLine();
        }
    }

    // �����Ի������� UI ������״̬
    private void EndDialogue()
    {
        isInDialogue = false;
        hasDialogueEnded = true;
        currentLineIndex = 0;
        HideDialogueUI();
    }

    // ��ʾ�Ի� UI
    private void ShowDialogueUI()
    {
        dialogueImage.gameObject.SetActive(true);
        dialogueText.gameObject.SetActive(true);
    }

    // ���ضԻ� UI
    private void HideDialogueUI()
    {
        dialogueImage.gameObject.SetActive(false);
        dialogueText.gameObject.SetActive(false);
    }
}