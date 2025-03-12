using UnityEngine;
using System.Collections.Generic;

public class EnemyCollision : MonoBehaviour
{
    public FadeManager fadeManager; // ���� FadeManager �ű�

    // ֧�ֵĵ��˱�ǩ�б�
    private List<string> enemyTags = new List<string> { "enemy", "Boss" };

    private void OnTriggerEnter(Collider other)
    {
        // ����Ƿ���ײ��֧�ֵĵ�������
        foreach (string tag in enemyTags)
        {
            if (other.CompareTag(tag))
            {
                // ��¼��Ҵ�������ʱ��λ��
                Vector3 playerPosition = transform.position;
                PlayerPrefs.SetFloat("PlayerX", playerPosition.x);
                PlayerPrefs.SetFloat("PlayerY", playerPosition.y);
                PlayerPrefs.SetFloat("PlayerZ", playerPosition.z);

                // ���������ĵ���������Ϊ��ʶ
                PlayerPrefs.SetString("EnemyName", other.gameObject.name);

                // ���� FadeManager �еĵ��뷽��
                if (fadeManager != null)
                {
                    fadeManager.StartFadeIn();
                }
                break; // �ҵ�ƥ��ı�ǩ���˳�ѭ��
            }
        }
    }
}
