using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BattleMechanismManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText; // 显示战斗对话信息的文本
    public TMP_InputField answerInputField; // 输入答案的输入框
    public Button submitButton; // 提交答案的按钮
    public WordLibrary wordLibrary; // 存储单词的库
    public TextMeshProUGUI playerHealthText; // 显示玩家生命值的文本
    public Image playerHealthBarFill; // 显示玩家当前生命值的血条图片
    public TextMeshProUGUI monsterHealthText; // 显示怪物生命值的文本
    public Image monsterHealthBarFill; // 显示怪物当前生命值的血条图片
    public Button restartButton; // 重新开始战斗的按钮

    private WordPair currentWord;
    public float monsterHealth; // 怪物当前生命值
    public float playerHealth = 100f; // 玩家初始生命值
    private float damagePerCorrectAnswer; // 每答对一题伤害值
    private float damagePerWrongAnswer; // 每答错一题伤害值
    private System.Random randomGenerator = new System.Random();
    private float monsterMaxHealth; // 怪物最大生命值
    public float playerMaxHealth = 100f; // 玩家最大生命值

    // 战斗属性
    private int damageReduction = 0; // 当前伤害减免值
    public MonsterData monsterData; // 怪物数据
    private bool autoAnswerEnabled = false; // 是否启用自动回答
    private int attackBoost = 0; // 当前攻击加成

    // 关键属性，当前战斗的敌人名称
    private string currentEnemyName;

    private enum QuestionType
    {
        ChineseToEnglish, // 中文转英文
        EnglishToChinese, // 英文转中文
        SpellTheWord // 拼写单词
    }

    private void Start()
    {
        // 初始化怪物数据
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

        // 初始化 UI 事件监听器
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
        // 初始化怪物相关属性
        if (monsterData != null)
        {
            monsterHealth = monsterData.maxHealth;
            monsterMaxHealth = monsterData.maxHealth;
            damagePerCorrectAnswer = monsterData.damagePerCorrectAnswer;
            damagePerWrongAnswer = monsterData.damagePerWrongAnswer;
        }
    }

    // 处理回车键按下事件
    private void OnEnterKeyPressed(string input)
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            OnSubmitAnswer();
        }
    }

    // 处理输入框文本变化事件
    private void OnAnswerInputChanged(string input)
    {
        // 如果输入框不为空，提交按钮可用，否则禁用
        submitButton.interactable = !string.IsNullOrEmpty(input);
    }

    private void StartBattle()
    {
        ShowMonsterDialogue("oi，你遇到了小怪物，开始战斗吧，输入单词来攻击它！");
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
                    question = "翻译： " + currentWord.chineseMeaning;
                    break;
                case QuestionType.EnglishToChinese:
                    question = "翻译： " + currentWord.englishWord;
                    break;
                case QuestionType.SpellTheWord:
                    string partialWord = GetPartialWord(currentWord.englishWord);
                    question = "请拼写这个单词，提示： " + partialWord + " " + currentWord.chineseMeaning;
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
        if (fullWord.Length <= 2) return fullWord; // 如果单词长度小于等于2，直接返回原单词

        int numLettersToHide = randomGenerator.Next(1, Mathf.Max(2, fullWord.Length - 1)); // 确定要隐藏的字母数量
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
            // 自动回答逻辑
            autoAnswerEnabled = false; // 重置标志位
            HandleCorrectAnswer();
        }
        else if (CheckAnswer(playerAnswer))
        {
            HandleCorrectAnswer();
        }
        else
        {
            TakeDamage((int)damagePerWrongAnswer); // 使用 TakeDamage 函数处理伤害
            if (playerHealth <= 0)
            {
                playerHealth = 0;
                ShowMonsterDialogue("战斗失败，游戏结束，请重新开始战斗");
                UpdatePlayerHealthText();
                UpdatePlayerHealthBar();
                submitButton.interactable = false; // 禁用提交按钮
            }
            else
            {
                // 获取正确答案
                string correctAnswer = "";
                string currentQuestion = dialogueText.text;
                if (currentQuestion.Contains("翻译： " + currentWord.chineseMeaning))
                {
                    correctAnswer = currentWord.englishWord;
                }
                else if (currentQuestion.Contains("翻译： " + currentWord.englishWord))
                {
                    correctAnswer = currentWord.chineseMeaning;
                }
                else if (currentQuestion.Contains("请拼写这个单词，提示： "))
                {
                    correctAnswer = currentWord.englishWord;
                }

                ShowMonsterDialogue($"回答错误，正确答案是：{correctAnswer}，你剩余生命值： {playerHealth}");
                UpdatePlayerHealthText();
                UpdatePlayerHealthBar();
                StartCoroutine(WaitForMouseClickAfterAnswer());
            }
        }
        answerInputField.text = ""; // 清空输入框
        OnAnswerInputChanged(answerInputField.text);
    }

    // 处理正确答案逻辑
    private void HandleCorrectAnswer()
    {
        int damage = (int)damagePerCorrectAnswer + attackBoost; // 增加攻击伤害
        monsterHealth -= damage;
        if (monsterHealth <= 0)
        {
            monsterHealth = 0;

            // 保存击败的敌人名称
            SaveDefeatedEnemyName();

            ShowMonsterDialogue("怪物被击败，你胜利了！");
            submitButton.interactable = false; // 禁用提交按钮

            // 随机生成 100 - 200 的金币数量
            int randomGold = randomGenerator.Next(100, 201);
            // 添加获得的金币
            BackpackManager.Instance.AddMoney(randomGold);
            Debug.Log($"你获得了 {randomGold} 金币，当前总金币数为{BackpackManager.Instance.money}");

            ReturnToScene1();// 战斗胜利后返回场景1
        }
        else
        {
            ShowMonsterDialogue($"回答正确，怪物剩余生命值： {monsterHealth}");
            StartCoroutine(WaitForMouseClickAfterAnswer());
        }
    }

    private void ReturnToScene1()
    {
        // 设置战斗胜利标志
        PlayerPrefs.SetInt("BattleWon", 1);

        // 返回场景1
        UnityEngine.SceneManagement.SceneManager.LoadScene("1");
    }

    private bool CheckAnswer(string playerAnswer)
    {
        if (currentWord == null) return false;

        string correctAnswer;
        string currentQuestion = dialogueText.text;
        if (currentQuestion.Contains("翻译： " + currentWord.chineseMeaning))
        {
            correctAnswer = currentWord.englishWord;
            return playerAnswer == correctAnswer;
        }
        else if (currentQuestion.Contains("翻译： " + currentWord.englishWord))
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
        else if (currentQuestion.Contains("请拼写这个单词，提示： "))
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
            playerHealthText.text = "玩家生命值: " + playerHealth;
        }
    }

    // 更新玩家血条显示
    public void UpdatePlayerHealthBar()
    {
        if (playerHealthBarFill != null)
        {
            float fillRatio = playerHealth / playerMaxHealth;
            playerHealthBarFill.fillAmount = fillRatio;
        }
    }

    // 更新怪物生命值文本显示
    public void UpdateMonsterHealthText()
    {
        if (monsterHealthText != null)
        {
            monsterHealthText.text = "怪物生命值: " + monsterHealth;
        }
    }

    // 更新怪物血条显示
    public void UpdateMonsterHealthBar()
    {
        if (monsterHealthBarFill != null)
        {
            float fillRatio = monsterHealth / monsterMaxHealth;
            monsterHealthBarFill.fillAmount = fillRatio;
        }
    }

    // 重新开始战斗
    private void RestartBattle()
    {
        // 恢复怪物和玩家的初始生命值
        monsterHealth = monsterMaxHealth;
        playerHealth = playerMaxHealth;

        // 恢复背包状态
        if (FightingSceneLoader.backpackStateBeforeBattle != null)
        {
            BackpackManager.Instance.RestoreItems(FightingSceneLoader.backpackStateBeforeBattle);
        }
        else
        {
            Debug.LogWarning("未找到战斗前的背包状态，使用初始状态");
            BackpackManager.Instance.ResetItemsToInitial(); // 作为备用方法
        }

        // 启用提交按钮
        submitButton.interactable = true;

        // 清空输入框
        answerInputField.text = "";

        // 更新生命值文本和血条显示
        UpdatePlayerHealthText();
        UpdatePlayerHealthBar();
        UpdateMonsterHealthText();
        UpdateMonsterHealthBar();

        // 开始新的战斗
        StartBattle();
    }

    // 恢复玩家生命值
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

    // 处理玩家受到的伤害
    public void TakeDamage(int damage)
    {
        int finalDamage = damage - damageReduction; // 应用伤害减免
        finalDamage = Mathf.Max(finalDamage, 0); // 确保伤害值非负

        playerHealth -= finalDamage;
        if (playerHealth < 0)
        {
            playerHealth = 0;
        }

        damageReduction = 0; // 重置伤害减免
        UpdatePlayerHealthText();
        UpdatePlayerHealthBar();

        Debug.Log("Player took " + finalDamage + " damage. Current health: " + playerHealth);
    }

    // 设置伤害减免
    public void SetDamageReduction(int amount)
    {
        damageReduction = amount;
        Debug.Log("Damage reduction set to " + amount + ".");
    }

    // 启用自动回答
    public void EnableAutoAnswer()
    {
        autoAnswerEnabled = true;
        // 立即提交答案以触发正确回答逻辑
        OnSubmitAnswer();
    }

    // 对敌人造成伤害
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

            // 保存击败的敌人名称
            SaveDefeatedEnemyName();

            ShowMonsterDialogue("怪物被击败，你胜利了！");
            submitButton.interactable = false; // 禁用提交按钮

            // 随机生成 100 - 200 的金币数量
            int randomGold = randomGenerator.Next(100, 201);
            // 添加获得的金币
            BackpackManager.Instance.AddMoney(randomGold);
            Debug.Log($"你获得了 {randomGold} 金币，当前总金币数为{BackpackManager.Instance.money}");

            ReturnToScene1(); // 战斗胜利后返回场景1
        }
    }

    // 增加攻击加成
    public void IncreaseAttack(int amount)
    {
        attackBoost = amount;
        Debug.Log("Attack increased by " + amount + " for this turn.");
    }

    // 重置攻击加成
    public void ResetAttackBoost()
    {
        attackBoost = 0;
        Debug.Log("Attack boost reset.");
    }

    // 设置当前敌人名称
    public void SetCurrentEnemyName(string name)
    {
        currentEnemyName = name;
    }

    private void SaveDefeatedEnemyName()
    {
        Debug.Log($"进入保存逻辑，当前 currentEnemyName：{currentEnemyName}"); // 新增日志
        if (!string.IsNullOrEmpty(currentEnemyName))
        {
            string defeatedEnemies = PlayerPrefs.GetString("DefeatedEnemies", "");
            Debug.Log($"保存前已有的 defeatedEnemies：{defeatedEnemies}"); // 新增日志
            if (!defeatedEnemies.Contains(currentEnemyName))
            {
                defeatedEnemies = string.IsNullOrEmpty(defeatedEnemies)
                    ? currentEnemyName
                     : $"{defeatedEnemies},{currentEnemyName}";
                PlayerPrefs.SetString("DefeatedEnemies", defeatedEnemies);
                Debug.Log($"最终保存的 defeatedEnemies：{defeatedEnemies}"); // 新增日志
            }
        }
    }
}