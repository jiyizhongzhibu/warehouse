using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public AudioSource audioSource; // 背景音乐的音频源
    public Slider volumeSlider; // 音量滑动条

    void Start()
    {
        // 初始化滑动条的值
        if (audioSource != null && volumeSlider != null)
        {
            volumeSlider.value = audioSource.volume;
        }

        // 绑定滑动条事件
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    // 当滑动条值改变时调用
    private void OnVolumeChanged(float value)
    {
        if (audioSource != null)
        {
            audioSource.volume = value; // 调整音量
        }
    }
}