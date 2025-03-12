using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class ItemPickup : MonoBehaviour
{
    public ItemData itemToAdd;
    public int itemAmount = 1;
    public int moneyToAdd = 0;

    [Header("UI Settings")]
    public GameObject pickupMessagePanel; // 在编辑器中拖入创建好的UI面板
    public TMP_Text pickupMessageText;
    public float displayTime = 2f;

    private bool isProcessing = false;

    private void Start()
    {
        // 在游戏开始时隐藏提示框
        if (pickupMessagePanel != null)
        {
            pickupMessagePanel.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isProcessing)
        {
            isProcessing = true;
            StartCoroutine(PickupProcess());
        }
    }

    IEnumerator PickupProcess()
    {
        // 添加物品到背包
        BackpackManager backpackManager = BackpackManager.Instance;
        if (backpackManager != null)
        {
            if (itemToAdd != null) backpackManager.AddItem(itemToAdd, itemAmount);
            if (moneyToAdd > 0) backpackManager.AddMoney(moneyToAdd);
        }

        // 显示提示信息
        if (pickupMessagePanel != null && pickupMessageText != null)
        {
            // 构建消息
            string message = "";
            if (itemToAdd != null) message = $"获得 {itemToAdd.itemName} ×{itemAmount}";
            if (moneyToAdd > 0) message = (message != "" ? "\n" : "") + $"获得金币 ×{moneyToAdd}";

            if (!string.IsNullOrEmpty(message))
            {
                pickupMessagePanel.SetActive(true);
                pickupMessageText.text = message;

                // 等待显示时间
                yield return new WaitForSeconds(displayTime);

                // 隐藏提示
                pickupMessagePanel.SetActive(false);
            }
        }

        // 最后执行销毁
        RecordDestroyedModelName(gameObject.name);
        Destroy(gameObject);
    }

    void RecordDestroyedModelName(string modelName)
    {
        // 保持原有记录逻辑不变
        string key = "DestroyedModels";
        string existingNames = PlayerPrefs.GetString(key, "");
        existingNames = string.IsNullOrEmpty(existingNames) ? modelName : $"{existingNames},{modelName}";
        PlayerPrefs.SetString(key, existingNames);
        PlayerPrefs.Save();
    }
}