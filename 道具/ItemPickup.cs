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
    public GameObject pickupMessagePanel; // �ڱ༭�������봴���õ�UI���
    public TMP_Text pickupMessageText;
    public float displayTime = 2f;

    private bool isProcessing = false;

    private void Start()
    {
        // ����Ϸ��ʼʱ������ʾ��
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
        // �����Ʒ������
        BackpackManager backpackManager = BackpackManager.Instance;
        if (backpackManager != null)
        {
            if (itemToAdd != null) backpackManager.AddItem(itemToAdd, itemAmount);
            if (moneyToAdd > 0) backpackManager.AddMoney(moneyToAdd);
        }

        // ��ʾ��ʾ��Ϣ
        if (pickupMessagePanel != null && pickupMessageText != null)
        {
            // ������Ϣ
            string message = "";
            if (itemToAdd != null) message = $"��� {itemToAdd.itemName} ��{itemAmount}";
            if (moneyToAdd > 0) message = (message != "" ? "\n" : "") + $"��ý�� ��{moneyToAdd}";

            if (!string.IsNullOrEmpty(message))
            {
                pickupMessagePanel.SetActive(true);
                pickupMessageText.text = message;

                // �ȴ���ʾʱ��
                yield return new WaitForSeconds(displayTime);

                // ������ʾ
                pickupMessagePanel.SetActive(false);
            }
        }

        // ���ִ������
        RecordDestroyedModelName(gameObject.name);
        Destroy(gameObject);
    }

    void RecordDestroyedModelName(string modelName)
    {
        // ����ԭ�м�¼�߼�����
        string key = "DestroyedModels";
        string existingNames = PlayerPrefs.GetString(key, "");
        existingNames = string.IsNullOrEmpty(existingNames) ? modelName : $"{existingNames},{modelName}";
        PlayerPrefs.SetString(key, existingNames);
        PlayerPrefs.Save();
    }
}