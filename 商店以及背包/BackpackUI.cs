using UnityEngine;
using TMPro;

public class BackpackUI : MonoBehaviour
{
    // 新增：单例实例
    public static BackpackUI Instance { get; private set; }

    // 金币显示文本
    [Header("金币显示")]
    public TMP_Text coinText;

    // 道具显示 - 生命之心
    [Header("道具显示")]
    public ItemData lifeHeartItem;    // 生命之心道具数据
    public TMP_Text lifeHeartText;    // 生命之心数量文本

    // 道具显示 - 力量战斧
    public ItemData powerAxeItem;     // 力量战斧道具数据
    public TMP_Text powerAxeText;     // 力量战斧数量文本

    // 道具显示 - 毒药
    public ItemData poisonItem;       // 毒药道具数据
    public TMP_Text poisonText;       // 毒药数量文本

    // 道具显示 - 白银护甲
    public ItemData silverArmorItem;  // 白银护甲道具数据
    public TMP_Text silverArmorText;  // 白银护甲数量文本

    // 道具显示 - 灵感药水
    public ItemData inspirationPotionItem; // 灵感药水道具数据
    public TMP_Text inspirationPotionText; // 灵感药水数量文本

    private void Awake()
    {
        // 单例初始化
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        RefreshUI(); // 游戏启动时刷新UI
    }

    private void OnEnable()
    {
        RefreshUI(); // 界面激活时刷新UI（如打开背包界面）
    }

    public void RefreshUI()
    {
        // 刷新金币数量
        UpdateCoinDisplay();

        // 刷新各个道具数量
        UpdateItemDisplay(lifeHeartItem, lifeHeartText);
        UpdateItemDisplay(powerAxeItem, powerAxeText);
        UpdateItemDisplay(poisonItem, poisonText);
        UpdateItemDisplay(silverArmorItem, silverArmorText);
        UpdateItemDisplay(inspirationPotionItem, inspirationPotionText);
    }

    private void UpdateCoinDisplay()
    {
        if (coinText != null)
        {
            coinText.text = BackpackManager.Instance.money.ToString();
        }
    }

    private void UpdateItemDisplay(ItemData itemData, TMP_Text textComponent)
    {
        if (itemData == null || textComponent == null) return;

        int quantity = BackpackManager.Instance.GetItemQuantity(itemData);
        textComponent.text = quantity.ToString();
        Debug.Log($"[UI调试] {itemData.itemName} 显示数量：{quantity}"); // 新增调试日志，实时反馈UI显示的数量
    }
}