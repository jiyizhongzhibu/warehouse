using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    public Image fadeImage; // ���뵭�������ֲ�ͼƬ
    public float fadeDuration = 1f; // ���뵭���ĳ���ʱ��
    public string fightingSceneName = "fighting"; // ս����������

    private void Awake()
    {
        // ȷ�� FadeManager �ڳ����л�ʱ��������
        //DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // ��ʼʱ���ֲ���ȫ͸��
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0f);
        // ���ĳ�����������¼�
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void StartFadeIn()
    {
        // ֹͣ��ǰ�������ڽ��е�Э��
        StopAllCoroutines();
        // ��ʼ����Ч��
        StartCoroutine(FadeIn());
    }

    private System.Collections.IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // ������ɺ��л�����
        SceneManager.LoadScene(fightingSceneName);
    }

    private System.Collections.IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == fightingSceneName)
        {
            // ��ս������������ɺ󣬿�ʼ����Ч��
            StartCoroutine(FadeOut());
        }
    }

    private void OnDestroy()
    {
        // ȡ�����ĳ�����������¼�
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}