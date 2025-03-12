using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class NPCDialogue : MonoBehaviour
{
    // �Ի�������Χ
    public float dialogueRange = 3f;
    // Ŀ��λ�ã���һ�ζԻ��������ƶ������ﴥ���ڶ��ζԻ�
    public Transform targetPosition;
    // ����Ի�λ�ã��ⲿ��������Ի�ʱ�ƶ�������
    public Transform specialDialoguePosition;
    // ���յص�
    public Transform finalPosition;
    // ������ʾ�Ի��ı��� UI ���
    public TextMeshProUGUI dialogueText;
    // �Ի����ͼƬ UI ���
    public Image dialogueImage;
    // ���ý�����
    public GameObject endingPanel;
    // NPC �Ķ���������
    public Animator animator;

    // ��һ�ζԻ���������
    private string[] firstDialogueLines = new string[] {
        "Զ�������˰�����ӭ����˫�������ı߾���",
        "������Ƭ���ص������߰�ɯ��",
        "�ܱ�Ǹ�������ִٵķ�ʽ��������������ǵ�����������ǰ��δ�е�Σ�� ���� �Ǹ��Գ� ' ����ħ�� ' �ļһ�úڰ�ħ���������������������������С�",
        "���ǵ����������ᣬ�����ֶ��ڽ���������...",
        "��˵ֻ������˫������֮�������߲����ƽ�ħ��������������������ŵĶ�����Ϣ����������ѡ�е�֤����",
        "��Ը�����������",
        "���������"
    };

    // �ڶ��ζԻ���������
    private string[] secondDialogueLines = new string[] {
        "С�ģ�ǰ��Ѳ�ߵİ�Ӱ������ħ���Ķ�Ŀ��",
        "���ǵ�����������������С�",
        "��ȥ�ӽ�����������ȥ������ɣ�",
        "�¸ҵؿ����ɣ�ð���ߣ�",
        "׼��������̽��֮�ü�����ʼ��"
    };

    // ����Ի���������
    private string[] specialDialogueLines = new string[] {
        "���Ѿ�����˵�һ��ս��������Ŷ��",
        "����������Ҳ�����һЩ��ҡ�",
        "ÿ�δ�ܹ���󶼻��ý��Ŷ��",
        "�㿪���Ͻǵı��������Բ鿴���е���Դ",
        "���Ͻǵ��̵���Ի��ѽ�ҹ�����ߡ�",
        "������ս����ʹ�õ��ߣ�������ͬ�ĵ���Ч��Ŷ��",
        "���������ð��֮��˳��һЩ��",
        "��Ҫע�⣬ֻ�д�����еĹ����ħ���Ż�����",
        "��ʱ���һ����һ������ħ����",
        "�������ͣ�ȥ������ʹ���ɣ�"
    };

    // ���նԻ���������
    private string[] finalDialogueLines = new string[] {
        "���Ѿ����������еĵ��ˣ�����̫���ˣ�",
        "���ڣ�������һ��ȥ��ս����ħ���ɣ�",
        "�����������ľ�ս������ʼ��"
    };

    // ��ֶԻ���������
    private string[] endingDialogueLines = new string[] {
        "������Ǵ����ħ�������������ǡ�",
        "˫����������Զ������Ķ��顣",
        "��������ǵ�Ӣ�ۣ�"
    };

    // ��ǰ�Ի���������
    private int currentLineIndex = 0;
    // �Ƿ����ڽ��жԻ�
    private bool isInDialogue = false;
    // ��һ�ζԻ��Ƿ����
    public bool isFirstDialogueCompleted = false;
    // �ڶ��ζԻ��Ƿ����
    public bool isSecondDialogueCompleted = false;
    // ����Ի��Ƿ����
    public bool isSpecialDialogueCompleted = false;
    // ���նԻ��Ƿ����
    public bool isFinalDialogueCompleted = false;
    // ��ֶԻ��Ƿ����
    public bool isEndingDialogueCompleted = false;
    // �Ƿ������ƶ���Ŀ��λ��
    private bool isMovingToTarget = false;
    // �Ƿ������ƶ�������Ի�λ��
    private bool isMovingToSpecialPosition = false;
    // �Ƿ������ƶ�������λ��
    private bool isMovingToFinalPosition = false;
    // �Ƿ������ƶ������ǰ��
    private bool isMovingToPlayerFront = false;
    // �Ƿ�������ͨ���˶�������
    private bool allEnemiesDefeated = false;
    // �Ƿ����е��ˣ����� Boss����������
    private bool allEnemiesAndBossDefeated = false;
    // NPC �ĵ�������������
    private NavMeshAgent navMeshAgent;
    private GameObject player;

    private string[] currentDialogueLines;

    void Start()
    {
        // ��ȡ��������������
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent == null)
        {
            Debug.LogError("NPC ȱ�� NavMeshAgent �����");
            enabled = false;
            return;
        }
        // ������Ҷ�������
        player = GameObject.FindGameObjectWithTag("Player");
        // ���ضԻ�����ı�
        HideDialogueUI();
        // ��ʼ�������ٶ�Ϊ 0
        if (animator != null)
        {
            animator.SetFloat("speed", 0f);
        }

        // ����������������¼�
        SceneManager.sceneLoaded += OnSceneLoaded;

        // �� PlayerPrefs ��ȡ�Ի�״̬
        isFirstDialogueCompleted = PlayerPrefs.GetInt("IsFirstDialogueCompleted", 0) == 1;
        isSecondDialogueCompleted = PlayerPrefs.GetInt("IsSecondDialogueCompleted", 0) == 1;
        isSpecialDialogueCompleted = PlayerPrefs.GetInt("IsSpecialDialogueCompleted", 0) == 1;
        isFinalDialogueCompleted = PlayerPrefs.GetInt("IsFinalDialogueCompleted", 0) == 1;
        isEndingDialogueCompleted = PlayerPrefs.GetInt("IsEndingDialogueCompleted", 0) == 1;

        // ����Ի�������˲�Ƶ�Ŀ��λ��
        if (isSpecialDialogueCompleted)
        {
            TeleportToSpecialDialoguePosition();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ���²�����Ҷ���
        player = GameObject.FindGameObjectWithTag("Player");

        // ���� NavMeshAgent ״̬
        if (navMeshAgent != null)
        {
            navMeshAgent.ResetPath();
        }

        // �ٴδ� PlayerPrefs ��ȡ�Ի�״̬
        isFirstDialogueCompleted = PlayerPrefs.GetInt("IsFirstDialogueCompleted", 0) == 1;
        isSecondDialogueCompleted = PlayerPrefs.GetInt("IsSecondDialogueCompleted", 0) == 1;
        isSpecialDialogueCompleted = PlayerPrefs.GetInt("IsSpecialDialogueCompleted", 0) == 1;
        isFinalDialogueCompleted = PlayerPrefs.GetInt("IsFinalDialogueCompleted", 0) == 1;
        isEndingDialogueCompleted = PlayerPrefs.GetInt("IsEndingDialogueCompleted", 0) == 1;
    }

    void Update()
    {
        // ���¶���״̬
        UpdateAnimation();

        // ������һ�ζԻ�
        TryStartFirstDialogue();

        // �����������Ի�
        HandleDialogueAdvancement();

        // ����Ŀ��λ�ú󴥷��ڶ��ζԻ�
        CheckIfReachedTargetPosition();

        // ��������Ի�λ�ú󴥷�����Ի�
        CheckIfReachedSpecialPosition();

        // ����Ƿ�������ͨ���˶�������
        CheckEnemiesDefeated();

        // ����Ƿ����е��ˣ����� Boss����������
        CheckAllEnemiesAndBossDefeated();

        // ��������λ�ú����߼�
        CheckIfReachedFinalPosition();

        // ����Ƿ񵽴����ǰ��
        CheckIfReachedPlayerFront();
    }

    // ���¶���״̬�������ƶ��ٶ����ö�������
    private void UpdateAnimation()
    {
        if (animator != null)
        {
            animator.SetFloat("speed", navMeshAgent.velocity.magnitude > 0.1f ? 1f : 0f);
        }
    }

    // ���Դ�����һ�ζԻ�
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

    // �����������Ի��߼�
    private void HandleDialogueAdvancement()
    {
        if (isInDialogue && Input.GetMouseButtonDown(0))
        {
            AdvanceDialogue();
        }
    }

    // ����Ƿ񵽴�Ŀ��λ�ò������ڶ��ζԻ�
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

    // ����Ƿ񵽴�����Ի�λ�ò���������Ի�
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

    // ����Ƿ�������ͨ���˶�������
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

    // ����Ƿ����е��ˣ����� Boss����������
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

    // ˲�Ƶ����ǰ�� 10 �״����ƶ�
    private void MoveToPlayerFront()
    {
        if (player != null)
        {
            Vector3 direction = player.transform.forward.normalized;
            Vector3 targetPos = player.transform.position + direction * 10f;
            navMeshAgent.Warp(targetPos);
            navMeshAgent.ResetPath();

            // ���õ���Ŀ��Ϊ������� 2 �״�
            SetDestinationNearPlayer();
            isMovingToPlayerFront = true;
        }
    }

    private void SetDestinationNearPlayer()
    {
        if (player != null)
        {
            // ���������� 2 �״���λ��
            Vector3 directionToPlayer = (player.transform.position - navMeshAgent.transform.position).normalized;
            Vector3 targetPos = player.transform.position - directionToPlayer * 2f;

            // ���õ���Ŀ��
            navMeshAgent.SetDestination(targetPos);
        }
    }

    // ����Ƿ񵽴����ǰ��
    private void CheckIfReachedPlayerFront()
    {
        if (isMovingToPlayerFront && !navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            isMovingToPlayerFront = false;
            FacePlayer();
        }
    }

    // ����Ƿ񵽴�����λ��
    private void CheckIfReachedFinalPosition()
    {
        if (isMovingToFinalPosition && !navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            isMovingToFinalPosition = false;
        }
    }

    // ��ʼ�Ի��������Ӧ�ĶԻ���������
    private void StartDialogue(string[] dialogueLines)
    {
        isInDialogue = true;
        currentLineIndex = 0;
        currentDialogueLines = dialogueLines; // ��¼��ǰ�Ի�����
        ShowDialogueUI();
        FacePlayer(); // ��ʼ�Ի�ʱ�泯���
        DisplayDialogueLine(dialogueLines);
    }

    // ��ʾ��ǰ�Ի���
    private void DisplayDialogueLine(string[] dialogueLines)
    {
        if (currentLineIndex < dialogueLines.Length)
        {
            dialogueText.text = dialogueLines[currentLineIndex];
        }
    }

    // �ƽ��Ի�����ʾ��һ��
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

    // �����Ի������� UI ������״̬
    private void EndDialogue()
    {
        isInDialogue = false;
        currentLineIndex = 0;
        HideDialogueUI();
        // ���������κζԻ������״̬

        // ���ݵ�ǰ��ɵĶԻ����ñ�־
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
            // ���������
            if (endingPanel != null)
            {
                endingPanel.SetActive(true);
            }
        }

        PlayerPrefs.Save();

    }

    // �ƶ���Ŀ��λ��
    private void MoveToTargetPosition()
    {
        if (navMeshAgent == null || !navMeshAgent.enabled) return;
        isMovingToTarget = true;
        navMeshAgent.SetDestination(targetPosition.position);
    }

    // �ⲿ���õĿ�ʼ����Ի�����
    public void StartSpecialDialogue()
    {
        if (!isSpecialDialogueCompleted && !isFinalDialogueCompleted)
        {
            // ˲�Ƶ�Ŀ��λ��
            if (navMeshAgent == null || !navMeshAgent.enabled) return;
            navMeshAgent.Warp(targetPosition.position);
            navMeshAgent.ResetPath();
            isMovingToSpecialPosition = true;
            navMeshAgent.SetDestination(specialDialoguePosition.position);
        }
    }

    // �������
    private void FacePlayer()
    {
        if (player != null)
        {
            Vector3 direction = player.transform.position - transform.position;
            direction.y = 0;
            transform.rotation = Quaternion.LookRotation(direction);
        }
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

    // �ƶ�������λ��
    private void MoveToFinalPosition()
    {
        if (navMeshAgent == null || !navMeshAgent.enabled) return;
        isMovingToFinalPosition = true;
        navMeshAgent.SetDestination(finalPosition.position);
    }

    // ˲�Ƶ�����Ի�λ��
    public void TeleportToSpecialDialoguePosition()
    {
        if (navMeshAgent == null || !navMeshAgent.enabled) return;
        if (specialDialoguePosition == null)
        {
            Debug.LogError("Special dialogue position is not set!");
            return;
        }

        // ֹͣ��ǰ�ĵ���·��
        navMeshAgent.ResetPath();
        // ˲�Ƶ�����Ի�λ��
        navMeshAgent.Warp(specialDialoguePosition.position);
        // �����ƶ�״̬
        isMovingToSpecialPosition = false;
        isMovingToTarget = false;
        isMovingToFinalPosition = false;
    }
}