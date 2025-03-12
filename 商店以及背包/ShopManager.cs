using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [System.Serializable]
    public class ShopItem
    {
        public ItemData item;
        public int price;
        public int stock;
        public int currentSelectCount;
        public TMP_Text countText;
        public TMP_Text stockText;
        public Button addButton;
        public Button reduceButton;
    }

    public ShopItem[] shopItems;
    public TMP_Text coinText;
    public TMP_Text totalCostText;
    public Button buyButton;
    public DialogBox dialogBox;

    private float lastRefreshTime;
    public float refreshInterval = 300f;

    private void Start()
    {
        InitializeShop();
        RefreshShopStock();
        UpdateCoinDisplay();
        buyButton.onClick.AddListener(OnBuyButtonClick);
    }

    private void OnEnable()
    {
        if (Time.time - lastRefreshTime >= refreshInterval)
        {
            RefreshShopStock();
            lastRefreshTime = Time.time;
        }
    }

    void InitializeShop()
    {
        foreach (var item in shopItems)
        {
            item.currentSelectCount = 0;
            item.countText.text = "0";

            //ʹ����ʱ��������ǰѭ����
            var tempItem = item;

            item.addButton.onClick.AddListener(() => ChangeItemCount(tempItem, 1));
            item.reduceButton.onClick.AddListener(() => ChangeItemCount(tempItem, -1));
        }
        UpdateTotalCost();
    }

    void RefreshShopStock()
    {
        foreach (var item in shopItems)
        {
            item.stock = Random.Range(1, 6);
            item.stockText.text = $"���: {item.stock}";
        }
    }

    void ChangeItemCount(ShopItem item, int changeValue)
    {
        item.currentSelectCount = Mathf.Clamp(item.currentSelectCount + changeValue, 0, item.stock);
        item.countText.text = item.currentSelectCount.ToString();
        UpdateTotalCost();
    }

    void UpdateTotalCost()
    {
        int total = 0;
        foreach (var item in shopItems)
        {
            total += item.price * item.currentSelectCount;
        }
        totalCostText.text = $"������: {total} ���";
    }

    void OnBuyButtonClick()
    {
        int totalCost = 0;
        foreach (var item in shopItems)
        {
            totalCost += item.price * item.currentSelectCount;
        }

        if (BackpackManager.Instance.CanAfford(totalCost))
        {
            BackpackManager.Instance.SpendMoney(totalCost);
            string successMessage = $"�ɹ�������Ʒ�������� {totalCost} ���";

            foreach (var item in shopItems)
            {
                if (item.currentSelectCount > 0)
                {
                    item.stock -= item.currentSelectCount;
                    item.stockText.text = $"���: {item.stock}";
                    BackpackManager.Instance.AddItem(item.item, item.currentSelectCount);
                }
            }

            dialogBox.ShowSuccessDialog(successMessage);
            if (BackpackUI.Instance != null)
            {
                BackpackUI.Instance.RefreshUI();
            }
            InitializeShop();
            UpdateCoinDisplay();
        }
        else
        {
            dialogBox.ShowErrorDialog("��Ҳ��㣬�޷�����");
        }
    }

    void UpdateCoinDisplay()
    {
        if (coinText != null)
        {
            coinText.text = $"���: {BackpackManager.Instance.money}";
        }
    }
}