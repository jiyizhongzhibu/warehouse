using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject menuPanel; // ��ק�󶨲˵����

    void Update()
    {
        // ��ⰴ������
        if (Input.GetKeyDown(KeyCode.Tab)) // ʹ�� Tab ����/�رղ˵�
        {
            ToggleMenu();
        }
    }

    // �л��˵���ʾ״̬
    void ToggleMenu()
    {
        menuPanel.SetActive(!menuPanel.activeSelf); // �л�����״̬
    }
}