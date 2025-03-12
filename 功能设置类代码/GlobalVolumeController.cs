using UnityEngine;
using UnityEngine.UI;

public class GlobalVolumeController : MonoBehaviour
{
    public Slider volumeSlider; // ����������

    void Start()
    {
        // ��ʼ����������ֵ
        if (volumeSlider != null)
        {
            volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f); // Ĭ��ֵΪ1
            AudioListener.volume = volumeSlider.value;
        }

        // �󶨻������¼�
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    // ��������ֵ�ı�ʱ����
    private void OnVolumeChanged(float value)
    {
        AudioListener.volume = value; // ����ȫ������
        PlayerPrefs.SetFloat("Volume", value); // ������������
        PlayerPrefs.Save();
    }
}