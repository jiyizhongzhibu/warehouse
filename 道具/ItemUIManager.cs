using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class ItemUIManager : MonoBehaviour
{
    [Header("道具切换UI组件")]
    [SerializeField] private Image 道具图标;
    [SerializeField] private Button 向左切换按钮;
    [SerializeField] private Button 向右切换按钮;
    [SerializeField] private TMP_Text 显示数量文本;
    [SerializeField] private TMP_Text 显示名称文本;
    [SerializeField] private Button 使用按钮;

    [Header("道具使用提示")]
    [SerializeField] private TMP_Text 使用提示文本;
    [SerializeField] private float 提示显示时间 = 2f;
    [SerializeField] private float 渐隐时间 = 1f;

    private int 当前道具索引 = 0;
    private List<ItemData> 所有道具列表 = new List<ItemData>();
    private bool 正在显示提示 = false;

    private void Start()
    {
        if (BackpackManager.Instance == null)
        {
            Debug.LogError("BackpackManager 未找到！请确保场景中有 BackpackManager 实例。");
            return;
        }

        // 初始化按钮事件
        向左切换按钮.onClick.AddListener(向左切换);
        向右切换按钮.onClick.AddListener(向右切换);
        使用按钮.onClick.AddListener(使用当前道具);

        // 初始化道具列表
        UpdateItemList();
        UpdateItemDisplay();

        // 隐藏提示文本
        使用提示文本.gameObject.SetActive(false);
    }

    private void 向左切换()
    {
        当前道具索引 = Mathf.Max(0, 当前道具索引 - 1);
        UpdateItemDisplay();
    }

    private void 向右切换()
    {
        当前道具索引 = Mathf.Min(所有道具列表.Count - 1, 当前道具索引 + 1);
        UpdateItemDisplay();
    }

    private void 使用当前道具()
    {
        if (所有道具列表.Count == 0)
        {
            StartCoroutine(显示使用提示("没有可用道具"));
            return;
        }

        var 当前道具 = 所有道具列表[当前道具索引];
        int 数量 = BackpackManager.Instance.GetItemQuantity(当前道具);
        if (数量 <= 0) return;

        string 使用结果 = 当前道具.UseItem();
        StartCoroutine(显示使用提示(使用结果));
        UpdateItemList();
        UpdateItemDisplay();
    }

    private IEnumerator 显示使用提示(string message)
    {
        if (正在显示提示) yield break;
        正在显示提示 = true;

        使用提示文本.text = message;
        使用提示文本.gameObject.SetActive(true);
        使用提示文本.color = message.Contains("成功") ? Color.green : Color.red;

        yield return new WaitForSeconds(提示显示时间 - 渐隐时间);

        float elapsedTime = 0f;
        Color startColor = 使用提示文本.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        while (elapsedTime < 渐隐时间)
        {
            elapsedTime += Time.deltaTime;
            使用提示文本.color = Color.Lerp(startColor, endColor, elapsedTime / 渐隐时间);
            yield return null;
        }

        使用提示文本.gameObject.SetActive(false);
        正在显示提示 = false;
    }

    public void RefreshUI()
    {
        UpdateItemList();
        UpdateItemDisplay();
    }

    private void UpdateItemDisplay()
    {
        if (所有道具列表.Count == 0)
        {
            道具图标.gameObject.SetActive(false);
            显示数量文本.text = "0";
            显示名称文本.text = "无道具";
            向左切换按钮.interactable = false;
            向右切换按钮.interactable = false;
            使用按钮.interactable = false;
            return;
        }

        var 当前道具 = 所有道具列表[当前道具索引];
        int 数量 = BackpackManager.Instance.GetItemQuantity(当前道具);

        道具图标.sprite = 当前道具.icon;
        道具图标.gameObject.SetActive(true);
        道具图标.color = 数量 > 0 ? Color.white : Color.gray;
        显示名称文本.text = 当前道具.itemName;
        显示数量文本.text = 数量.ToString();
        使用按钮.interactable = 数量 > 0;
        向左切换按钮.interactable = 当前道具索引 > 0;
        向右切换按钮.interactable = 当前道具索引 < 所有道具列表.Count - 1;
    }

    private void UpdateItemList()
    {
        所有道具列表 = new List<ItemData>(BackpackManager.Instance.GetAllItemsInBackpack().Keys);
        当前道具索引 = Mathf.Clamp(当前道具索引, 0, 所有道具列表.Count - 1);
    }
}