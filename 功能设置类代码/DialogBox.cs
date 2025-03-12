using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogBox : MonoBehaviour
{
    [Header("�ı����")]
    public TMP_Text dialogText; // �ϲ�����ı����

    [Header("��ʽ����")]
    public Color successColor = Color.green;  // �ɹ���ʾ��ɫ
    public Color errorColor = Color.red;      // ʧ����ʾ��ɫ
    public float displayDuration = 3f;        // ��ʾ����ʱ��

    private void Start()
    {
        gameObject.SetActive(false); // ��ʼ����
    }

    /// <summary>
    /// ��ʾ�ɹ���ʾ
    /// </summary>
    /// <param name="message">������ʾ��Ϣ��������������ݣ�</param>
    public void ShowSuccessDialog(string message)
    {
        dialogText.color = successColor;
        dialogText.text = message;
        ShowDialog();
    }

    /// <summary>
    /// ��ʾ������ʾ
    /// </summary>
    /// <param name="message">������ʾ��Ϣ��������������ݣ�</param>
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