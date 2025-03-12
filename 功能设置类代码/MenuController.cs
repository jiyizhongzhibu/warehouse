using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject menuPanel; // 拖拽绑定菜单面板

    void Update()
    {
        // 检测按键输入
        if (Input.GetKeyDown(KeyCode.Tab)) // 使用 Tab 键打开/关闭菜单
        {
            ToggleMenu();
        }
    }

    // 切换菜单显示状态
    void ToggleMenu()
    {
        menuPanel.SetActive(!menuPanel.activeSelf); // 切换激活状态
    }
}