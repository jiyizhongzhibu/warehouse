using UnityEngine;
using UnityEngine.Events;

public class EnemyDeathTrigger : MonoBehaviour
{
    [Header("�¼�����")]
    public UnityEvent OnEnemyDestroyed; // ����Unity�¼�

    [Header("NPC����")]
    public NPCDialogue targetNPC; // ��Ҫ�����Ի���NPC

    void OnDestroy()
    {
        // �����¼����༭���󶨻������ã�
        OnEnemyDestroyed?.Invoke();

        // ������ֶ�ָ��NPC��ֱ�ӵ����䷽��
        if (targetNPC != null)
        {
            targetNPC.StartSpecialDialogue();
        }
    }
}
