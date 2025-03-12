using UnityEngine;
using UnityEngine.SceneManagement;

public class StartLoader : MonoBehaviour
{
    private const string BossSpawnedKey = "BossSpawned";

    public void SwitchToScene1()
    {
        // ��ʼ�����жԻ���־
        PlayerPrefs.SetInt("IsFirstDialogueCompleted", 0);
        PlayerPrefs.SetInt("IsSecondDialogueCompleted", 0);
        PlayerPrefs.SetInt("IsSpecialDialogueCompleted", 0);
        PlayerPrefs.SetInt("IsFinalDialogueCompleted", 0);
        PlayerPrefs.SetInt("IsEndingDialogueCompleted", 0);

        // ��յ��˺͵��ߵĳ־û���¼
        PlayerPrefs.SetString("DefeatedEnemies", "");
        PlayerPrefs.SetString("DestroyedModels", "");  // �����ؼ�����

        // ����Boss״̬
        PlayerPrefs.SetInt(BossSpawnedKey, 0);

        // ǿ�Ʊ��������޸�
        PlayerPrefs.Save();
        Debug.Log("�������л����ѳ�ʼ������״̬��\n" +
                 "- �Ի���־����\n" +
                 "- ����/���߼�¼���\n" +
                 "- Bossδ����");

        // ���س���
        SceneManager.LoadScene("1");
    }
}