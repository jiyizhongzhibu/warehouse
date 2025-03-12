using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class ItemUIManager : MonoBehaviour
{
    [Header("�����л�UI���")]
    [SerializeField] private Image ����ͼ��;
    [SerializeField] private Button �����л���ť;
    [SerializeField] private Button �����л���ť;
    [SerializeField] private TMP_Text ��ʾ�����ı�;
    [SerializeField] private TMP_Text ��ʾ�����ı�;
    [SerializeField] private Button ʹ�ð�ť;

    [Header("����ʹ����ʾ")]
    [SerializeField] private TMP_Text ʹ����ʾ�ı�;
    [SerializeField] private float ��ʾ��ʾʱ�� = 2f;
    [SerializeField] private float ����ʱ�� = 1f;

    private int ��ǰ�������� = 0;
    private List<ItemData> ���е����б� = new List<ItemData>();
    private bool ������ʾ��ʾ = false;

    private void Start()
    {
        if (BackpackManager.Instance == null)
        {
            Debug.LogError("BackpackManager δ�ҵ�����ȷ���������� BackpackManager ʵ����");
            return;
        }

        // ��ʼ����ť�¼�
        �����л���ť.onClick.AddListener(�����л�);
        �����л���ť.onClick.AddListener(�����л�);
        ʹ�ð�ť.onClick.AddListener(ʹ�õ�ǰ����);

        // ��ʼ�������б�
        UpdateItemList();
        UpdateItemDisplay();

        // ������ʾ�ı�
        ʹ����ʾ�ı�.gameObject.SetActive(false);
    }

    private void �����л�()
    {
        ��ǰ�������� = Mathf.Max(0, ��ǰ�������� - 1);
        UpdateItemDisplay();
    }

    private void �����л�()
    {
        ��ǰ�������� = Mathf.Min(���е����б�.Count - 1, ��ǰ�������� + 1);
        UpdateItemDisplay();
    }

    private void ʹ�õ�ǰ����()
    {
        if (���е����б�.Count == 0)
        {
            StartCoroutine(��ʾʹ����ʾ("û�п��õ���"));
            return;
        }

        var ��ǰ���� = ���е����б�[��ǰ��������];
        int ���� = BackpackManager.Instance.GetItemQuantity(��ǰ����);
        if (���� <= 0) return;

        string ʹ�ý�� = ��ǰ����.UseItem();
        StartCoroutine(��ʾʹ����ʾ(ʹ�ý��));
        UpdateItemList();
        UpdateItemDisplay();
    }

    private IEnumerator ��ʾʹ����ʾ(string message)
    {
        if (������ʾ��ʾ) yield break;
        ������ʾ��ʾ = true;

        ʹ����ʾ�ı�.text = message;
        ʹ����ʾ�ı�.gameObject.SetActive(true);
        ʹ����ʾ�ı�.color = message.Contains("�ɹ�") ? Color.green : Color.red;

        yield return new WaitForSeconds(��ʾ��ʾʱ�� - ����ʱ��);

        float elapsedTime = 0f;
        Color startColor = ʹ����ʾ�ı�.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        while (elapsedTime < ����ʱ��)
        {
            elapsedTime += Time.deltaTime;
            ʹ����ʾ�ı�.color = Color.Lerp(startColor, endColor, elapsedTime / ����ʱ��);
            yield return null;
        }

        ʹ����ʾ�ı�.gameObject.SetActive(false);
        ������ʾ��ʾ = false;
    }

    public void RefreshUI()
    {
        UpdateItemList();
        UpdateItemDisplay();
    }

    private void UpdateItemDisplay()
    {
        if (���е����б�.Count == 0)
        {
            ����ͼ��.gameObject.SetActive(false);
            ��ʾ�����ı�.text = "0";
            ��ʾ�����ı�.text = "�޵���";
            �����л���ť.interactable = false;
            �����л���ť.interactable = false;
            ʹ�ð�ť.interactable = false;
            return;
        }

        var ��ǰ���� = ���е����б�[��ǰ��������];
        int ���� = BackpackManager.Instance.GetItemQuantity(��ǰ����);

        ����ͼ��.sprite = ��ǰ����.icon;
        ����ͼ��.gameObject.SetActive(true);
        ����ͼ��.color = ���� > 0 ? Color.white : Color.gray;
        ��ʾ�����ı�.text = ��ǰ����.itemName;
        ��ʾ�����ı�.text = ����.ToString();
        ʹ�ð�ť.interactable = ���� > 0;
        �����л���ť.interactable = ��ǰ�������� > 0;
        �����л���ť.interactable = ��ǰ�������� < ���е����б�.Count - 1;
    }

    private void UpdateItemList()
    {
        ���е����б� = new List<ItemData>(BackpackManager.Instance.GetAllItemsInBackpack().Keys);
        ��ǰ�������� = Mathf.Clamp(��ǰ��������, 0, ���е����б�.Count - 1);
    }
}