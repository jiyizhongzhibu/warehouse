using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class NPCDialogue : MonoBehaviour
{
    // 对话触发范围
    public float dialogueRange = 3f;
    // 目标位置，第一次对话结束后移动到这里触发第二次对话
    public Transform targetPosition;
    // 特殊对话位置，外部调用特殊对话时移动到这里
    public Transform specialDialoguePosition;
    // 最终地点
    public Transform finalPosition;
    // 用于显示对话文本的 UI 组件
    public TextMeshProUGUI dialogueText;
    // 对话框的图片 UI 组件
    public Image dialogueImage;
    // 引用结局面板
    public GameObject endingPanel;
    // NPC 的动画控制器
    public Animator animator;

    // 第一次对话内容数组
    private string[] firstDialogueLines = new string[] {
        "远方的旅人啊，欢迎来到双语王国的边境。",
        "我是这片土地的引导者艾莎。",
        "很抱歉用这样仓促的方式与你相见，但我们的王国正面临前所未有的危机 —— 那个自称 ' 语咒魔王 ' 的家伙，用黑暗魔法将半数居民囚禁在语言牢笼中。",
        "他们的声音被剥夺，连名字都在渐渐被遗忘...",
        "传说只有掌握双生语言之力的勇者才能破解魔王的咒语，而你身上流动着的独特气息，正是命运选中的证明。",
        "你愿意帮助我们吗？",
        "快跟我来。"
    };

    // 第二次对话内容数组
    private string[] secondDialogueLines = new string[] {
        "小心！前方巡逻的暗影守卫是魔王的耳目。",
        "它们的弱点藏在语言破绽中。",
        "快去接近他，让我们去打败他吧！",
        "勇敢地靠近吧，冒险者！",
        "准备好了吗，探索之旅即将开始。"
    };

    // 特殊对话内容数组
    private string[] specialDialogueLines = new string[] {
        "你已经完成了第一次战斗，不错哦！",
        "哇塞，我们也获得了一些金币。",
        "每次打败怪物后都会获得金币哦。",
        "点开左上角的背包，可以查看现有的资源",
        "左上角的商店可以花费金币购买道具。",
        "可以在战斗中使用道具，触发不同的道具效果哦。",
        "这会让你们冒险之旅顺利一些。",
        "但要注意，只有打败所有的怪物，大魔王才会现身。",
        "到时候我会和你一起征讨魔王！",
        "继续加油，去完成你的使命吧！"
    };

    // 最终对话内容数组
    private string[] finalDialogueLines = new string[] {
        "你已经消灭了所有的敌人，做得太棒了！",
        "现在，让我们一起去挑战语咒魔王吧！",
        "跟我来，最后的决战即将开始！"
    };

    // 结局对话内容数组
    private string[] endingDialogueLines = new string[] {
        "你帮我们打败了魔王，拯救了我们。",
        "双语王国将永远铭记你的恩情。",
        "你就是我们的英雄！"
    };

    // 当前对话的行索引
    private int currentLineIndex = 0;
    // 是否正在进行对话
    private bool isInDialogue = false;
    // 第一次对话是否完成
    public bool isFirstDialogueCompleted = false;
    // 第二次对话是否完成
    public bool isSecondDialogueCompleted = false;
    // 特殊对话是否完成
    public bool isSpecialDialogueCompleted = false;
    // 最终对话是否完成
    public bool isFinalDialogueCompleted = false;
    // 结局对话是否完成
    public bool isEndingDialogueCompleted = false;
    // 是否正在移动到目标位置
    private bool isMovingToTarget = false;
    // 是否正在移动到特殊对话位置
    private bool isMovingToSpecialPosition = false;
    // 是否正在移动到最终位置
    private bool isMovingToFinalPosition = false;
    // 是否正在移动到玩家前方
    private bool isMovingToPlayerFront = false;
    // 是否所有普通敌人都被消灭
    private bool allEnemiesDefeated = false;
    // 是否所有敌人（包括 Boss）都被消灭
    private bool allEnemiesAndBossDefeated = false;
    // NPC 的导航网格代理组件
    private NavMeshAgent navMeshAgent;
    private GameObject player;

    private string[] currentDialogueLines;

    void Start()
    {
        // 获取导航网格代理组件
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent == null)
        {
            Debug.LogError("NPC 缺少 NavMeshAgent 组件！");
            enabled = false;
            return;
        }
        // 缓存玩家对象引用
        player = GameObject.FindGameObjectWithTag("Player");
        // 隐藏对话框和文本
        HideDialogueUI();
        // 初始化动画速度为 0
        if (animator != null)
        {
            animator.SetFloat("speed", 0f);
        }

        // 监听场景加载完成事件
        SceneManager.sceneLoaded += OnSceneLoaded;

        // 从 PlayerPrefs 读取对话状态
        isFirstDialogueCompleted = PlayerPrefs.GetInt("IsFirstDialogueCompleted", 0) == 1;
        isSecondDialogueCompleted = PlayerPrefs.GetInt("IsSecondDialogueCompleted", 0) == 1;
        isSpecialDialogueCompleted = PlayerPrefs.GetInt("IsSpecialDialogueCompleted", 0) == 1;
        isFinalDialogueCompleted = PlayerPrefs.GetInt("IsFinalDialogueCompleted", 0) == 1;
        isEndingDialogueCompleted = PlayerPrefs.GetInt("IsEndingDialogueCompleted", 0) == 1;

        // 特殊对话结束后，瞬移到目标位置
        if (isSpecialDialogueCompleted)
        {
            TeleportToSpecialDialoguePosition();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 重新查找玩家对象
        player = GameObject.FindGameObjectWithTag("Player");

        // 重置 NavMeshAgent 状态
        if (navMeshAgent != null)
        {
            navMeshAgent.ResetPath();
        }

        // 再次从 PlayerPrefs 读取对话状态
        isFirstDialogueCompleted = PlayerPrefs.GetInt("IsFirstDialogueCompleted", 0) == 1;
        isSecondDialogueCompleted = PlayerPrefs.GetInt("IsSecondDialogueCompleted", 0) == 1;
        isSpecialDialogueCompleted = PlayerPrefs.GetInt("IsSpecialDialogueCompleted", 0) == 1;
        isFinalDialogueCompleted = PlayerPrefs.GetInt("IsFinalDialogueCompleted", 0) == 1;
        isEndingDialogueCompleted = PlayerPrefs.GetInt("IsEndingDialogueCompleted", 0) == 1;
    }

    void Update()
    {
        // 更新动画状态
        UpdateAnimation();

        // 触发第一次对话
        TryStartFirstDialogue();

        // 处理点击继续对话
        HandleDialogueAdvancement();

        // 到达目标位置后触发第二次对话
        CheckIfReachedTargetPosition();

        // 到达特殊对话位置后触发特殊对话
        CheckIfReachedSpecialPosition();

        // 检查是否所有普通敌人都被消灭
        CheckEnemiesDefeated();

        // 检查是否所有敌人（包括 Boss）都被消灭
        CheckAllEnemiesAndBossDefeated();

        // 到达最终位置后处理逻辑
        CheckIfReachedFinalPosition();

        // 检查是否到达玩家前方
        CheckIfReachedPlayerFront();
    }

    // 更新动画状态，根据移动速度设置动画参数
    private void UpdateAnimation()
    {
        if (animator != null)
        {
            animator.SetFloat("speed", navMeshAgent.velocity.magnitude > 0.1f ? 1f : 0f);
        }
    }

    // 尝试触发第一次对话
    private void TryStartFirstDialogue()
    {
        if (!isFirstDialogueCompleted && !isInDialogue)
        {
            if (player != null && Vector3.Distance(transform.position, player.transform.position) < dialogueRange)
            {
                StartDialogue(firstDialogueLines);
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

    // 检查是否到达目标位置并触发第二次对话
    private void CheckIfReachedTargetPosition()
    {
        if (isMovingToTarget && !navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            isMovingToTarget = false;

            if (!isSecondDialogueCompleted)
            {
                StartDialogue(secondDialogueLines);
            }
        }
    }

    // 检查是否到达特殊对话位置并触发特殊对话
    private void CheckIfReachedSpecialPosition()
    {
        if (isMovingToSpecialPosition && !navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            isMovingToSpecialPosition = false;
            FacePlayer();

            if (!isSpecialDialogueCompleted)
            {
                StartDialogue(specialDialogueLines);

            }
        }
    }

    // 检查是否所有普通敌人都被消灭
    private void CheckEnemiesDefeated()
    {
        if (!allEnemiesDefeated && GameObject.FindGameObjectsWithTag("enemy").Length == 0)
        {
            MoveToPlayerFront();
            allEnemiesDefeated = true;

            if (!isFinalDialogueCompleted)
            {
                StartDialogue(finalDialogueLines);

            }
        }
    }

    // 检查是否所有敌人（包括 Boss）都被消灭
    private void CheckAllEnemiesAndBossDefeated()
    {
        if (!allEnemiesAndBossDefeated && GameObject.FindGameObjectsWithTag("enemy").Length == 0 && GameObject.FindGameObjectsWithTag("Boss").Length == 0)
        {
            MoveToPlayerFront();
            allEnemiesAndBossDefeated = true;

            if (!isEndingDialogueCompleted)
            {
                StartDialogue(endingDialogueLines);


            }
        }
    }

    // 瞬移到玩家前方 10 米处后移动
    private void MoveToPlayerFront()
    {
        if (player != null)
        {
            Vector3 direction = player.transform.forward.normalized;
            Vector3 targetPos = player.transform.position + direction * 10f;
            navMeshAgent.Warp(targetPos);
            navMeshAgent.ResetPath();

            // 设置导航目标为距离玩家 2 米处
            SetDestinationNearPlayer();
            isMovingToPlayerFront = true;
        }
    }

    private void SetDestinationNearPlayer()
    {
        if (player != null)
        {
            // 计算距离玩家 2 米处的位置
            Vector3 directionToPlayer = (player.transform.position - navMeshAgent.transform.position).normalized;
            Vector3 targetPos = player.transform.position - directionToPlayer * 2f;

            // 设置导航目标
            navMeshAgent.SetDestination(targetPos);
        }
    }

    // 检查是否到达玩家前方
    private void CheckIfReachedPlayerFront()
    {
        if (isMovingToPlayerFront && !navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            isMovingToPlayerFront = false;
            FacePlayer();
        }
    }

    // 检查是否到达最终位置
    private void CheckIfReachedFinalPosition()
    {
        if (isMovingToFinalPosition && !navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            isMovingToFinalPosition = false;
        }
    }

    // 开始对话，传入对应的对话内容数组
    private void StartDialogue(string[] dialogueLines)
    {
        isInDialogue = true;
        currentLineIndex = 0;
        currentDialogueLines = dialogueLines; // 记录当前对话数组
        ShowDialogueUI();
        FacePlayer(); // 开始对话时面朝玩家
        DisplayDialogueLine(dialogueLines);
    }

    // 显示当前对话行
    private void DisplayDialogueLine(string[] dialogueLines)
    {
        if (currentLineIndex < dialogueLines.Length)
        {
            dialogueText.text = dialogueLines[currentLineIndex];
        }
    }

    // 推进对话，显示下一行
    private void AdvanceDialogue()
    {
        if (navMeshAgent.velocity.magnitude > 0.1f) return;

        currentLineIndex++;
        if (currentLineIndex >= currentDialogueLines.Length)
        {
            EndDialogue();
        }
        else
        {
            DisplayDialogueLine(currentDialogueLines);
        }
    }

    // 结束对话，隐藏 UI 并重置状态
    private void EndDialogue()
    {
        isInDialogue = false;
        currentLineIndex = 0;
        HideDialogueUI();
        // 不再重置任何对话的完成状态

        // 根据当前完成的对话设置标志
        if (currentDialogueLines == firstDialogueLines)
        {
            isFirstDialogueCompleted = true;
            PlayerPrefs.SetInt("IsFirstDialogueCompleted", 1);
            MoveToTargetPosition();
        }
        else if (currentDialogueLines == secondDialogueLines)
        {
            isSecondDialogueCompleted = true;
            PlayerPrefs.SetInt("IsSecondDialogueCompleted", 1);
        }
        else if (currentDialogueLines == specialDialogueLines)
        {
            isSpecialDialogueCompleted = true;
            PlayerPrefs.SetInt("IsSpecialDialogueCompleted", 1);
        }
        else if (currentDialogueLines == finalDialogueLines)
        {
            isFinalDialogueCompleted = true;
            PlayerPrefs.SetInt("IsFinalDialogueCompleted", 1);
            MoveToFinalPosition();
        }
        else if (currentDialogueLines == endingDialogueLines)
        {
            isEndingDialogueCompleted = true;
            PlayerPrefs.SetInt("IsEndingDialogueCompleted", 1);
            // 激活结局面板
            if (endingPanel != null)
            {
                endingPanel.SetActive(true);
            }
        }

        PlayerPrefs.Save();

    }

    // 移动到目标位置
    private void MoveToTargetPosition()
    {
        if (navMeshAgent == null || !navMeshAgent.enabled) return;
        isMovingToTarget = true;
        navMeshAgent.SetDestination(targetPosition.position);
    }

    // 外部调用的开始特殊对话方法
    public void StartSpecialDialogue()
    {
        if (!isSpecialDialogueCompleted && !isFinalDialogueCompleted)
        {
            // 瞬移到目标位置
            if (navMeshAgent == null || !navMeshAgent.enabled) return;
            navMeshAgent.Warp(targetPosition.position);
            navMeshAgent.ResetPath();
            isMovingToSpecialPosition = true;
            navMeshAgent.SetDestination(specialDialoguePosition.position);
        }
    }

    // 面向玩家
    private void FacePlayer()
    {
        if (player != null)
        {
            Vector3 direction = player.transform.position - transform.position;
            direction.y = 0;
            transform.rotation = Quaternion.LookRotation(direction);
        }
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

    // 移动到最终位置
    private void MoveToFinalPosition()
    {
        if (navMeshAgent == null || !navMeshAgent.enabled) return;
        isMovingToFinalPosition = true;
        navMeshAgent.SetDestination(finalPosition.position);
    }

    // 瞬移到特殊对话位置
    public void TeleportToSpecialDialoguePosition()
    {
        if (navMeshAgent == null || !navMeshAgent.enabled) return;
        if (specialDialoguePosition == null)
        {
            Debug.LogError("Special dialogue position is not set!");
            return;
        }

        // 停止当前的导航路径
        navMeshAgent.ResetPath();
        // 瞬移到特殊对话位置
        navMeshAgent.Warp(specialDialoguePosition.position);
        // 重置移动状态
        isMovingToSpecialPosition = false;
        isMovingToTarget = false;
        isMovingToFinalPosition = false;
    }
}