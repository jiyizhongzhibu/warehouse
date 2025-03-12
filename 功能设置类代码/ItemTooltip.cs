using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ItemTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ItemData itemData; // 关联的道具数据
    public TMP_Text tooltipText; // 用于显示描述的文本组件

    private void Start()
    {
        // 初始化时隐藏描述文本
        if (tooltipText != null)
        {
            tooltipText.enabled = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemData != null && tooltipText != null)
        {
            // 显示描述文本
            tooltipText.enabled = true;
            // 设置描述文本
            tooltipText.text = itemData.description;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltipText != null)
        {
            // 隐藏描述文本
            tooltipText.enabled = false;
        }
    }
}
