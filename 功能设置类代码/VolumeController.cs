using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public AudioSource audioSource; // �������ֵ���ƵԴ
    public Slider volumeSlider; // ����������

    void Start()
    {
        // ��ʼ����������ֵ
        if (audioSource != null && volumeSlider != null)
        {
            volumeSlider.value = audioSource.volume;
        }

        // �󶨻������¼�
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    // ��������ֵ�ı�ʱ����
    private void OnVolumeChanged(float value)
    {
        if (audioSource != null)
        {
            audioSource.volume = value; // ��������
        }
    }
}