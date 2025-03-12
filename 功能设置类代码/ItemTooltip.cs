using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ItemTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ItemData itemData; // �����ĵ�������
    public TMP_Text tooltipText; // ������ʾ�������ı����

    private void Start()
    {
        // ��ʼ��ʱ���������ı�
        if (tooltipText != null)
        {
            tooltipText.enabled = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemData != null && tooltipText != null)
        {
            // ��ʾ�����ı�
            tooltipText.enabled = true;
            // ���������ı�
            tooltipText.text = itemData.description;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltipText != null)
        {
            // ���������ı�
            tooltipText.enabled = false;
        }
    }
}
