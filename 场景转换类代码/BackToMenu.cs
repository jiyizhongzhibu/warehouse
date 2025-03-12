using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackToMenu : MonoBehaviour
{
    // ���÷��ز˵��İ�ť
    public Button backToMenuButton;

    void Start()
    {
        // ��鰴ť�Ƿ�ֵ���󶨵���¼�
        if (backToMenuButton != null)
        {
            backToMenuButton.onClick.AddListener(LoadMenuScene);
        }
    }

    // ���ز˵������ķ���
    void LoadMenuScene()
    {
        SceneManager.LoadScene("menu");
    }
}