using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    public Image fadeImage; // 淡入淡出的遮罩层图片
    public float fadeDuration = 1f; // 淡入淡出的持续时间
    public string fightingSceneName = "fighting"; // 战斗场景名称

    private void Awake()
    {
        // 确保 FadeManager 在场景切换时不被销毁
        //DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // 初始时遮罩层完全透明
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0f);
        // 订阅场景加载完成事件
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void StartFadeIn()
    {
        // 停止当前可能正在进行的协程
        StopAllCoroutines();
        // 开始淡入效果
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
        // 淡入完成后切换场景
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
            // 当战斗场景加载完成后，开始淡出效果
            StartCoroutine(FadeOut());
        }
    }

    private void OnDestroy()
    {
        // 取消订阅场景加载完成事件
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}