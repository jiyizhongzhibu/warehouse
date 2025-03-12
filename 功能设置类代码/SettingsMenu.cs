using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public GameObject settingsPanel; // 拖拽绑定设置面板

    void Start()
    {
        // 确保面板初始状态为关闭
        settingsPanel.SetActive(false);
    }

    // 打开设置面板
    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    // 关闭设置面板
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }
}