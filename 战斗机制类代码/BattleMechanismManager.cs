using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BattleMechanismManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText; // ��ʾս���Ի���Ϣ���ı�
    public TMP_InputField answerInputField; // ����𰸵������
    public Button submitButton; // �ύ�𰸵İ�ť
    public WordLibrary wordLibrary; // �洢���ʵĿ�
    public TextMeshProUGUI playerHealthText; // ��ʾ�������ֵ���ı�
    public Image playerHealthBarFill; // ��ʾ��ҵ�ǰ����ֵ��Ѫ��ͼƬ
    public TextMeshProUGUI monsterHealthText; // ��ʾ��������ֵ���ı�
    public Image monsterHealthBarFill; // ��ʾ���ﵱǰ����ֵ��Ѫ��ͼƬ
    public Button restartButton; // ���¿�ʼս���İ�ť

    private WordPair currentWord;
    public float monsterHealth; // ���ﵱǰ����ֵ
    public float playerHealth = 100f; // ��ҳ�ʼ����ֵ
    private float damagePerCorrectAnswer; // ÿ���һ���˺�ֵ
    private float damagePerWrongAnswer; // ÿ���һ���˺�ֵ
    private System.Random randomGenerator = new System.Random();
    private float monsterMaxHealth; // �����������ֵ
    public float playerMaxHealth = 100f; // ����������ֵ

    // ս������
    private int damageReduction = 0; // ��ǰ�˺�����ֵ
    public MonsterData monsterData; // ��������
    private bool autoAnswerEnabled = false; // �Ƿ������Զ��ش�
    private int attackBoost = 0; // ��ǰ�����ӳ�

    // �ؼ����ԣ���ǰս���ĵ�������
    private string currentEnemyName;

    private enum QuestionType
    {
        ChineseToEnglish, // ����תӢ��
        EnglishToChinese, // Ӣ��ת����
        SpellTheWord // ƴд����
    }

    private void Start()
    {
        // ��ʼ����������
        if (monsterData != null)
        {
            monsterHealth = monsterData.maxHealth;
            monsterMaxHealth = monsterData.maxHealth;
            damagePerCorrectAnswer = monsterData.damagePerCorrectAnswer;
            damagePerWrongAnswer = monsterData.damagePerWrongAnswer;
        }
        else
        {
            Debug.LogError("MonsterData is not assigned!");
        }

        // ��ʼ�� UI �¼�������
        submitButton.onClick.AddListener(OnSubmitAnswer);
        answerInputField.onValueChanged.AddListener(OnAnswerInputChanged);
        OnAnswerInputChanged(answerInputField.text);
        restartButton.onClick.AddListener(RestartBattle);
        answerInputField.onEndEdit.AddListener(OnEnterKeyPressed);

        StartBattle();
        UpdatePlayerHealthText();
        UpdatePlayerHealthBar();
        UpdateMonsterHealthText();
        UpdateMonsterHealthBar();
    }

    public void InitializeMonster()
    {
        // ��ʼ�������������
        if (monsterData != null)
        {
            monsterHealth = monsterData.maxHealth;
            monsterMaxHealth = monsterData.maxHealth;
            damagePerCorrectAnswer = monsterData.damagePerCorrectAnswer;
            damagePerWrongAnswer = monsterData.damagePerWrongAnswer;
        }
    }

    // ����س��������¼�
    private void OnEnterKeyPressed(string input)
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            OnSubmitAnswer();
        }
    }

    // ����������ı��仯�¼�
    private void OnAnswerInputChanged(string input)
    {
        // ��������Ϊ�գ��ύ��ť���ã��������
        submitButton.interactable = !string.IsNullOrEmpty(input);
    }

    private void StartBattle()
    {
        ShowMonsterDialogue("oi����������С�����ʼս���ɣ����뵥������������");
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
        StartCoroutine(WaitForMouseClick());
    }

    private IEnumerator WaitForMouseClick()
    {
        bool clicked = false;
        while (!clicked)
        {
            if (Input.GetMouseButtonDown(0))
            {
                clicked = true;
                GetNewWord();
            }
            yield return null;
        }
    }

    private void ShowMonsterDialogue(string dialogueContent)
    {
        if (dialogueText != null)
        {
            dialogueText.text = dialogueContent;
        }
    }

    private void GetNewWord()
    {
        currentWord = wordLibrary.GetRandomWord();
        if (currentWord != null)
        {
            QuestionType randomQuestionType = (QuestionType)randomGenerator.Next(0, 3);
            string question;
            switch (randomQuestionType)
            {
                case QuestionType.ChineseToEnglish:
                    question = "���룺 " + currentWord.chineseMeaning;
                    break;
                case QuestionType.EnglishToChinese:
                    question = "���룺 " + currentWord.englishWord;
                    break;
                case QuestionType.SpellTheWord:
                    string partialWord = GetPartialWord(currentWord.englishWord);
                    question = "��ƴд������ʣ���ʾ�� " + partialWord + " " + currentWord.chineseMeaning;
                    break;
                default:
                    question = "";
                    break;
            }
            ShowMonsterDialogue(question);
        }
    }

    private string GetPartialWord(string fullWord)
    {
        if (fullWord.Length <= 2) return fullWord; // ������ʳ���С�ڵ���2��ֱ�ӷ���ԭ����

        int numLettersToHide = randomGenerator.Next(1, Mathf.Max(2, fullWord.Length - 1)); // ȷ��Ҫ���ص���ĸ����
        List<int> hiddenLetterIndices = new List<int>();
        while (hiddenLetterIndices.Count < numLettersToHide)
        {
            int index = randomGenerator.Next(0, fullWord.Length);
            if (!hiddenLetterIndices.Contains(index))
            {
                hiddenLetterIndices.Add(index);
            }
        }

        char[] partialWordArray = fullWord.ToCharArray();
        foreach (int index in hiddenLetterIndices)
        {
            partialWordArray[index] = '_';
        }

        return new string(partialWordArray);
    }

    private void OnSubmitAnswer()
    {
        string playerAnswer = answerInputField.text.Trim();
        if (autoAnswerEnabled)
        {
            // �Զ��ش��߼�
            autoAnswerEnabled = false; // ���ñ�־λ
            HandleCorrectAnswer();
        }
        else if (CheckAnswer(playerAnswer))
        {
            HandleCorrectAnswer();
        }
        else
        {
            TakeDamage((int)damagePerWrongAnswer); // ʹ�� TakeDamage ���������˺�
            if (playerHealth <= 0)
            {
                playerHealth = 0;
                ShowMonsterDialogue("ս��ʧ�ܣ���Ϸ�����������¿�ʼս��");
                UpdatePlayerHealthText();
                UpdatePlayerHealthBar();
                submitButton.interactable = false; // �����ύ��ť
            }
            else
            {
                // ��ȡ��ȷ��
                string correctAnswer = "";
                string currentQuestion = dialogueText.text;
                if (currentQuestion.Contains("���룺 " + currentWord.chineseMeaning))
                {
                    correctAnswer = currentWord.englishWord;
                }
                else if (currentQuestion.Contains("���룺 " + currentWord.englishWord))
                {
                    correctAnswer = currentWord.chineseMeaning;
                }
                else if (currentQuestion.Contains("��ƴд������ʣ���ʾ�� "))
                {
                    correctAnswer = currentWord.englishWord;
                }

                ShowMonsterDialogue($"�ش������ȷ���ǣ�{correctAnswer}����ʣ������ֵ�� {playerHealth}");
                UpdatePlayerHealthText();
                UpdatePlayerHealthBar();
                StartCoroutine(WaitForMouseClickAfterAnswer());
            }
        }
        answerInputField.text = ""; // ��������
        OnAnswerInputChanged(answerInputField.text);
    }

    // ������ȷ���߼�
    private void HandleCorrectAnswer()
    {
        int damage = (int)damagePerCorrectAnswer + attackBoost; // ���ӹ����˺�
        monsterHealth -= damage;
        if (monsterHealth <= 0)
        {
            monsterHealth = 0;

            // ������ܵĵ�������
            SaveDefeatedEnemyName();

            ShowMonsterDialogue("���ﱻ���ܣ���ʤ���ˣ�");
            submitButton.interactable = false; // �����ύ��ť

            // ������� 100 - 200 �Ľ������
            int randomGold = randomGenerator.Next(100, 201);
            // ��ӻ�õĽ��
            BackpackManager.Instance.AddMoney(randomGold);
            Debug.Log($"������ {randomGold} ��ң���ǰ�ܽ����Ϊ{BackpackManager.Instance.money}");

            ReturnToScene1();// ս��ʤ���󷵻س���1
        }
        else
        {
            ShowMonsterDialogue($"�ش���ȷ������ʣ������ֵ�� {monsterHealth}");
            StartCoroutine(WaitForMouseClickAfterAnswer());
        }
    }

    private void ReturnToScene1()
    {
        // ����ս��ʤ����־
        PlayerPrefs.SetInt("BattleWon", 1);

        // ���س���1
        UnityEngine.SceneManagement.SceneManager.LoadScene("1");
    }

    private bool CheckAnswer(string playerAnswer)
    {
        if (currentWord == null) return false;

        string correctAnswer;
        string currentQuestion = dialogueText.text;
        if (currentQuestion.Contains("���룺 " + currentWord.chineseMeaning))
        {
            correctAnswer = currentWord.englishWord;
            return playerAnswer == correctAnswer;
        }
        else if (currentQuestion.Contains("���룺 " + currentWord.englishWord))
        {
            correctAnswer = currentWord.chineseMeaning;
            foreach (char c in correctAnswer)
            {
                if (playerAnswer.Contains(c.ToString()))
                {
                    return true;
                }
            }
            return false;
        }
        else if (currentQuestion.Contains("��ƴд������ʣ���ʾ�� "))
        {
            correctAnswer = currentWord.englishWord;
            return playerAnswer == correctAnswer;
        }
        return false;
    }

    private IEnumerator WaitForMouseClickAfterAnswer()
    {
        bool clicked = false;
        while (!clicked)
        {
            if (Input.GetMouseButtonDown(0))
            {
                clicked = true;
                UpdateMonsterHealthText();
                UpdateMonsterHealthBar();
                UpdatePlayerHealthText();
                UpdatePlayerHealthBar();
                GetNewWord();
            }
            yield return null;
        }
    }

    public void UpdatePlayerHealthText()
    {
        if (playerHealthText != null)
        {
            playerHealthText.text = "�������ֵ: " + playerHealth;
        }
    }

    // �������Ѫ����ʾ
    public void UpdatePlayerHealthBar()
    {
        if (playerHealthBarFill != null)
        {
            float fillRatio = playerHealth / playerMaxHealth;
            playerHealthBarFill.fillAmount = fillRatio;
        }
    }

    // ���¹�������ֵ�ı���ʾ
    public void UpdateMonsterHealthText()
    {
        if (monsterHealthText != null)
        {
            monsterHealthText.text = "��������ֵ: " + monsterHealth;
        }
    }

    // ���¹���Ѫ����ʾ
    public void UpdateMonsterHealthBar()
    {
        if (monsterHealthBarFill != null)
        {
            float fillRatio = monsterHealth / monsterMaxHealth;
            monsterHealthBarFill.fillAmount = fillRatio;
        }
    }

    // ���¿�ʼս��
    private void RestartBattle()
    {
        // �ָ��������ҵĳ�ʼ����ֵ
        monsterHealth = monsterMaxHealth;
        playerHealth = playerMaxHealth;

        // �ָ�����״̬
        if (FightingSceneLoader.backpackStateBeforeBattle != null)
        {
            BackpackManager.Instance.RestoreItems(FightingSceneLoader.backpackStateBeforeBattle);
        }
        else
        {
            Debug.LogWarning("δ�ҵ�ս��ǰ�ı���״̬��ʹ�ó�ʼ״̬");
            BackpackManager.Instance.ResetItemsToInitial(); // ��Ϊ���÷���
        }

        // �����ύ��ť
        submitButton.interactable = true;

        // ��������
        answerInputField.text = "";

        // ��������ֵ�ı���Ѫ����ʾ
        UpdatePlayerHealthText();
        UpdatePlayerHealthBar();
        UpdateMonsterHealthText();
        UpdateMonsterHealthBar();

        // ��ʼ�µ�ս��
        StartBattle();
    }

    // �ָ��������ֵ
    public void RestorePlayerHealth(int amount)
    {
        playerHealth += amount;
        if (playerHealth > playerMaxHealth)
        {
            playerHealth = playerMaxHealth;
        }
        UpdatePlayerHealthText();
        UpdatePlayerHealthBar();
        Debug.Log("Player health restored by " + amount + ". Current health: " + playerHealth);
    }

    // ��������ܵ����˺�
    public void TakeDamage(int damage)
    {
        int finalDamage = damage - damageReduction; // Ӧ���˺�����
        finalDamage = Mathf.Max(finalDamage, 0); // ȷ���˺�ֵ�Ǹ�

        playerHealth -= finalDamage;
        if (playerHealth < 0)
        {
            playerHealth = 0;
        }

        damageReduction = 0; // �����˺�����
        UpdatePlayerHealthText();
        UpdatePlayerHealthBar();

        Debug.Log("Player took " + finalDamage + " damage. Current health: " + playerHealth);
    }

    // �����˺�����
    public void SetDamageReduction(int amount)
    {
        damageReduction = amount;
        Debug.Log("Damage reduction set to " + amount + ".");
    }

    // �����Զ��ش�
    public void EnableAutoAnswer()
    {
        autoAnswerEnabled = true;
        // �����ύ���Դ�����ȷ�ش��߼�
        OnSubmitAnswer();
    }

    // �Ե�������˺�
    public void DamageEnemy(int damageAmount)
    {
        monsterHealth -= damageAmount;
        if (monsterHealth < 0)
        {
            monsterHealth = 0;
        }
        UpdateMonsterHealthText();
        UpdateMonsterHealthBar();
        Debug.Log("Enemy took " + damageAmount + " damage. Current health: " + monsterHealth);

        if (monsterHealth <= 0)
        {
            monsterHealth = 0;

            // ������ܵĵ�������
            SaveDefeatedEnemyName();

            ShowMonsterDialogue("���ﱻ���ܣ���ʤ���ˣ�");
            submitButton.interactable = false; // �����ύ��ť

            // ������� 100 - 200 �Ľ������
            int randomGold = randomGenerator.Next(100, 201);
            // ��ӻ�õĽ��
            BackpackManager.Instance.AddMoney(randomGold);
            Debug.Log($"������ {randomGold} ��ң���ǰ�ܽ����Ϊ{BackpackManager.Instance.money}");

            ReturnToScene1(); // ս��ʤ���󷵻س���1
        }
    }

    // ���ӹ����ӳ�
    public void IncreaseAttack(int amount)
    {
        attackBoost = amount;
        Debug.Log("Attack increased by " + amount + " for this turn.");
    }

    // ���ù����ӳ�
    public void ResetAttackBoost()
    {
        attackBoost = 0;
        Debug.Log("Attack boost reset.");
    }

    // ���õ�ǰ��������
    public void SetCurrentEnemyName(string name)
    {
        currentEnemyName = name;
    }

    private void SaveDefeatedEnemyName()
    {
        Debug.Log($"���뱣���߼�����ǰ currentEnemyName��{currentEnemyName}"); // ������־
        if (!string.IsNullOrEmpty(currentEnemyName))
        {
            string defeatedEnemies = PlayerPrefs.GetString("DefeatedEnemies", "");
            Debug.Log($"����ǰ���е� defeatedEnemies��{defeatedEnemies}"); // ������־
            if (!defeatedEnemies.Contains(currentEnemyName))
            {
                defeatedEnemies = string.IsNullOrEmpty(defeatedEnemies)
                    ? currentEnemyName
                     : $"{defeatedEnemies},{currentEnemyName}";
                PlayerPrefs.SetString("DefeatedEnemies", defeatedEnemies);
                Debug.Log($"���ձ���� defeatedEnemies��{defeatedEnemies}"); // ������־
            }
        }
    }
}