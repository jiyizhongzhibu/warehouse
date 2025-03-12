using UnityEngine;
using UnityEngine.UI;

public class GlobalVolumeController : MonoBehaviour
{
    public Slider volumeSlider; // 音量滑动条

    void Start()
    {
        // 初始化滑动条的值
        if (volumeSlider != null)
        {
            volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f); // 默认值为1
            AudioListener.volume = volumeSlider.value;
        }

        // 绑定滑动条事件
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    // 当滑动条值改变时调用
    private void OnVolumeChanged(float value)
    {
        AudioListener.volume = value; // 调整全局音量
        PlayerPrefs.SetFloat("Volume", value); // 保存音量设置
        PlayerPrefs.Save();
    }
}