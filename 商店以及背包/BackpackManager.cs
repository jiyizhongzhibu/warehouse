using System.Collections.Generic;
using UnityEngine;

public class BackpackManager : MonoBehaviour
{
    // 单例实例
    private static BackpackManager _instance;
    public static BackpackManager Instance
    {
        get
        {
            // 如果实例为空，尝试查找
            if (_instance == null)
            {
                _instance = FindObjectOfType<BackpackManager>();
                if (_instance == null)
                {
                    Debug.LogError("BackpackManager 未找到！请确保场景中有 BackpackManager 实例。");
                }
            }
            return _instance;
        }
    }

    [Header("初始配置")]
    public int startingMoney = 100;
    public List<ItemData> startingItems = new List<ItemData>();
    public List<int> startingItemQuantities = new List<int>();

    private Dictionary<ItemData, int> itemDictionary = new Dictionary<ItemData, int>();
    private Dictionary<ItemData, int> initialItemDictionary = new Dictionary<ItemData, int>(); // 新增：存储初始道具数量
    public int money { get; private set; }
    private bool isInitialized = false; // 防止重复初始化

    private void Awake()
    {
        // 处理单例冲突
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // 设置单例实例
        _instance = this;
        DontDestroyOnLoad(gameObject);

        // 如果尚未初始化，则调用初始化
        if (!isInitialized)
        {
            InitializeBackpack();
            isInitialized = true; // 标记为已初始化
        }
    }

    private void InitializeBackpack()
    {
        money = startingMoney;
        Debug.Log("[背包初始化] 开始处理道具初始化");

        // 清空旧数据
        itemDictionary.Clear();
        initialItemDictionary.Clear(); // 清空初始字典

        // 遍历初始道具列表
        for (int i = 0; i < startingItems.Count; i++)
        {
            ItemData item = startingItems[i];
            int quantity = i < startingItemQuantities.Count ? startingItemQuantities[i] : 0;

            if (item != null)
            {
                itemDictionary[item] = quantity;
                initialItemDictionary[item] = quantity; // 记录初始数量
                Debug.Log($"[初始化成功] 已添加 {quantity} 个 {item.itemName}");
            }
            else
            {
                Debug.LogError($"[初始化错误] 索引 {i} 的道具为空！");
            }
        }
    }

    public void AddItem(ItemData item, int amount = 1)
    {
        if (item == null)
        {
            Debug.LogWarning("尝试操作空道具");
            return;
        }

        if (itemDictionary.ContainsKey(item))
        {
            itemDictionary[item] += amount;
            BackpackUI.Instance.RefreshUI(); 
        }
        else
        {
            itemDictionary[item] = amount;
        }

        Debug.Log($"[AddItem] 道具 {item.itemName} 数量更新为：{itemDictionary[item]}");
    }

    /// 获取背包中指定道具的数量
    public int GetItemQuantity(ItemData item)
    {
        return itemDictionary.TryGetValue(item, out int quantity) ? quantity : 0;
    }

    /// 判断金币是否足够支付指定金额
    public bool CanAfford(int cost)
    {
        return money >= cost;
    }

    /// 扣除金币
    public void SpendMoney(int amount)
    {
        if (CanAfford(amount))
        {
            money -= amount;
        }
    }

    // 公共方法用于增加金币
    public void AddMoney(int amount)
    {
        money += amount;
        BackpackUI.Instance.RefreshUI(); 
        Debug.Log($"当前金币增加 {amount}，总金币：{money}");
    }

    /// 获取背包中所有道具及其数量的信息（供切换场景等功能使用）
    public Dictionary<ItemData, int> GetAllItemsInBackpack()
    {
        // 返回字典副本，避免外部直接修改内部数据
        return new Dictionary<ItemData, int>(itemDictionary);
    }

    // 恢复道具数量到指定状态
    public void RestoreItems(Dictionary<ItemData, int> state)
    {
        itemDictionary = new Dictionary<ItemData, int>(state);
        Debug.Log("道具数量已恢复到战斗前的状态");
    }

    // 添加重置方法
    public void ResetItemsToInitial()
    {
        if (initialItemDictionary.Count == 0)
        {
            Debug.LogError("initialItemDictionary 未初始化！");
            return;
        }
        itemDictionary = new Dictionary<ItemData, int>(initialItemDictionary);
        Debug.Log("道具数量已重置为初始状态");
    }
}