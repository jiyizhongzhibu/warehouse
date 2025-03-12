using UnityEngine;
using TMPro;

public class BackpackUI : MonoBehaviour
{
    // ����������ʵ��
    public static BackpackUI Instance { get; private set; }

    // �����ʾ�ı�
    [Header("�����ʾ")]
    public TMP_Text coinText;

    // ������ʾ - ����֮��
    [Header("������ʾ")]
    public ItemData lifeHeartItem;    // ����֮�ĵ�������
    public TMP_Text lifeHeartText;    // ����֮�������ı�

    // ������ʾ - ����ս��
    public ItemData powerAxeItem;     // ����ս����������
    public TMP_Text powerAxeText;     // ����ս�������ı�

    // ������ʾ - ��ҩ
    public ItemData poisonItem;       // ��ҩ��������
    public TMP_Text poisonText;       // ��ҩ�����ı�

    // ������ʾ - ��������
    public ItemData silverArmorItem;  // �������׵�������
    public TMP_Text silverArmorText;  // �������������ı�

    // ������ʾ - ���ҩˮ
    public ItemData inspirationPotionItem; // ���ҩˮ��������
    public TMP_Text inspirationPotionText; // ���ҩˮ�����ı�

    private void Awake()
    {
        // ������ʼ��
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
        RefreshUI(); // ��Ϸ����ʱˢ��UI
    }

    private void OnEnable()
    {
        RefreshUI(); // ���漤��ʱˢ��UI����򿪱������棩
    }

    public void RefreshUI()
    {
        // ˢ�½������
        UpdateCoinDisplay();

        // ˢ�¸�����������
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
        Debug.Log($"[UI����] {itemData.itemName} ��ʾ������{quantity}"); // ����������־��ʵʱ����UI��ʾ������
    }
}