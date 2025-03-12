using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackToMenu : MonoBehaviour
{
    // 引用返回菜单的按钮
    public Button backToMenuButton;

    void Start()
    {
        // 检查按钮是否赋值，绑定点击事件
        if (backToMenuButton != null)
        {
            backToMenuButton.onClick.AddListener(LoadMenuScene);
        }
    }

    // 加载菜单场景的方法
    void LoadMenuScene()
    {
        SceneManager.LoadScene("menu");
    }
}