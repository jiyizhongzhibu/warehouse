using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public GameObject settingsPanel; // ��ק���������

    void Start()
    {
        // ȷ������ʼ״̬Ϊ�ر�
        settingsPanel.SetActive(false);
    }

    // ���������
    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    // �ر��������
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }
}