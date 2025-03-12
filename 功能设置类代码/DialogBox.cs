using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogBox : MonoBehaviour
{
    [Header("文本组件")]
    public TMP_Text dialogText; // 合并后的文本组件

    [Header("样式配置")]
    public Color successColor = Color.green;  // 成功提示颜色
    public Color errorColor = Color.red;      // 失败提示颜色
    public float displayDuration = 3f;        // 显示持续时间

    private void Start()
    {
        gameObject.SetActive(false); // 初始隐藏
    }

    /// <summary>
    /// 显示成功提示
    /// </summary>
    /// <param name="message">完整提示信息（包含标题和内容）</param>
    public void ShowSuccessDialog(string message)
    {
        dialogText.color = successColor;
        dialogText.text = message;
        ShowDialog();
    }

    /// <summary>
    /// 显示错误提示
    /// </summary>
    /// <param name="message">完整提示信息（包含标题和内容）</param>
    public void ShowErrorDialog(string message)
    {
        dialogText.color = errorColor;
        dialogText.text = message;
        ShowDialog();
    }

    private void ShowDialog()
    {
        gameObject.SetActive(true);
        Invoke(nameof(HideDialog), displayDuration);
    }

    private void HideDialog()
    {
        gameObject.SetActive(false);
    }
}