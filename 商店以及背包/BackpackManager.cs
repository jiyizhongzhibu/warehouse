using System.Collections.Generic;
using UnityEngine;

public class BackpackManager : MonoBehaviour
{
    // ����ʵ��
    private static BackpackManager _instance;
    public static BackpackManager Instance
    {
        get
        {
            // ���ʵ��Ϊ�գ����Բ���
            if (_instance == null)
            {
                _instance = FindObjectOfType<BackpackManager>();
                if (_instance == null)
                {
                    Debug.LogError("BackpackManager δ�ҵ�����ȷ���������� BackpackManager ʵ����");
                }
            }
            return _instance;
        }
    }

    [Header("��ʼ����")]
    public int startingMoney = 100;
    public List<ItemData> startingItems = new List<ItemData>();
    public List<int> startingItemQuantities = new List<int>();

    private Dictionary<ItemData, int> itemDictionary = new Dictionary<ItemData, int>();
    private Dictionary<ItemData, int> initialItemDictionary = new Dictionary<ItemData, int>(); // �������洢��ʼ��������
    public int money { get; private set; }
    private bool isInitialized = false; // ��ֹ�ظ���ʼ��

    private void Awake()
    {
        // ��������ͻ
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // ���õ���ʵ��
        _instance = this;
        DontDestroyOnLoad(gameObject);

        // �����δ��ʼ��������ó�ʼ��
        if (!isInitialized)
        {
            InitializeBackpack();
            isInitialized = true; // ���Ϊ�ѳ�ʼ��
        }
    }

    private void InitializeBackpack()
    {
        money = startingMoney;
        Debug.Log("[������ʼ��] ��ʼ������߳�ʼ��");

        // ��վ�����
        itemDictionary.Clear();
        initialItemDictionary.Clear(); // ��ճ�ʼ�ֵ�

        // ������ʼ�����б�
        for (int i = 0; i < startingItems.Count; i++)
        {
            ItemData item = startingItems[i];
            int quantity = i < startingItemQuantities.Count ? startingItemQuantities[i] : 0;

            if (item != null)
            {
                itemDictionary[item] = quantity;
                initialItemDictionary[item] = quantity; // ��¼��ʼ����
                Debug.Log($"[��ʼ���ɹ�] ����� {quantity} �� {item.itemName}");
            }
            else
            {
                Debug.LogError($"[��ʼ������] ���� {i} �ĵ���Ϊ�գ�");
            }
        }
    }

    public void AddItem(ItemData item, int amount = 1)
    {
        if (item == null)
        {
            Debug.LogWarning("���Բ����յ���");
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

        Debug.Log($"[AddItem] ���� {item.itemName} ��������Ϊ��{itemDictionary[item]}");
    }

    /// ��ȡ������ָ�����ߵ�����
    public int GetItemQuantity(ItemData item)
    {
        return itemDictionary.TryGetValue(item, out int quantity) ? quantity : 0;
    }

    /// �жϽ���Ƿ��㹻֧��ָ�����
    public bool CanAfford(int cost)
    {
        return money >= cost;
    }

    /// �۳����
    public void SpendMoney(int amount)
    {
        if (CanAfford(amount))
        {
            money -= amount;
        }
    }

    // ���������������ӽ��
    public void AddMoney(int amount)
    {
        money += amount;
        BackpackUI.Instance.RefreshUI(); 
        Debug.Log($"��ǰ������� {amount}���ܽ�ң�{money}");
    }

    /// ��ȡ���������е��߼�����������Ϣ�����л������ȹ���ʹ�ã�
    public Dictionary<ItemData, int> GetAllItemsInBackpack()
    {
        // �����ֵ丱���������ⲿֱ���޸��ڲ�����
        return new Dictionary<ItemData, int>(itemDictionary);
    }

    // �ָ�����������ָ��״̬
    public void RestoreItems(Dictionary<ItemData, int> state)
    {
        itemDictionary = new Dictionary<ItemData, int>(state);
        Debug.Log("���������ѻָ���ս��ǰ��״̬");
    }

    // ������÷���
    public void ResetItemsToInitial()
    {
        if (initialItemDictionary.Count == 0)
        {
            Debug.LogError("initialItemDictionary δ��ʼ����");
            return;
        }
        itemDictionary = new Dictionary<ItemData, int>(initialItemDictionary);
        Debug.Log("��������������Ϊ��ʼ״̬");
    }
}