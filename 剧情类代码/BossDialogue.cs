using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossDialogue : MonoBehaviour
{
    // 对话触发范围
    public float dialogueRange = 5f;
    // 用于显示对话文本的 UI 组件
    public TextMeshProUGUI dialogueText;
    // 对话框的图片 UI 组件
    public Image dialogueImage;

    // Boss 对话内容数组
    private string[] bossDialogueLines = new string[] {
        "哼，你这不知死活的家伙，竟敢闯入我的领地！",
        "今天就是你的末日，没有人能阻止我统治这片世界！",
        "在我强大的力量面前，你将不堪一击！",
        "准备好迎接死亡的降临吧！"
    };

    // 当前对话的行索引
    private int currentLineIndex = 0;
    // 是否正在进行对话
    private bool isInDialogue = false;
    // 对话是否已经结束过
    private bool hasDialogueEnded = false;

    void Start()
    {
        // 隐藏对话框和文本
        HideDialogueUI();
    }

    void Update()
    {
        // 检查是否存在 Boss 标签的对象且尝试触发对话
        if (IsBossPresent())
        {
            TryStartDialogue();
        }

        // 处理点击继续对话
        HandleDialogueAdvancement();
    }

    // 检查场景中是否存在带有 Boss 标签的对象
    private bool IsBossPresent()
    {
        GameObject[] bosses = GameObject.FindGameObjectsWithTag("Boss");
        return bosses.Length > 0;
    }

    // 尝试触发对话
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

    // 处理点击继续对话逻辑
    private void HandleDialogueAdvancement()
    {
        if (isInDialogue && Input.GetMouseButtonDown(0))
        {
            AdvanceDialogue();
        }
    }

    // 开始对话
    private void StartDialogue()
    {
        isInDialogue = true;
        hasDialogueEnded = false;
        currentLineIndex = 0;
        ShowDialogueUI();
        DisplayDialogueLine();
    }

    // 显示当前对话行
    private void DisplayDialogueLine()
    {
        if (currentLineIndex < bossDialogueLines.Length)
        {
            dialogueText.text = bossDialogueLines[currentLineIndex];
        }
    }

    // 推进对话，显示下一行
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

    // 结束对话，隐藏 UI 并重置状态
    private void EndDialogue()
    {
        isInDialogue = false;
        hasDialogueEnded = true;
        currentLineIndex = 0;
        HideDialogueUI();
    }

    // 显示对话 UI
    private void ShowDialogueUI()
    {
        dialogueImage.gameObject.SetActive(true);
        dialogueText.gameObject.SetActive(true);
    }

    // 隐藏对话 UI
    private void HideDialogueUI()
    {
        dialogueImage.gameObject.SetActive(false);
        dialogueText.gameObject.SetActive(false);
    }
}