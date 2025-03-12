using UnityEngine;
using UnityEngine.Events;

public class EnemyDeathTrigger : MonoBehaviour
{
    [Header("事件设置")]
    public UnityEvent OnEnemyDestroyed; // 定义Unity事件

    [Header("NPC引用")]
    public NPCDialogue targetNPC; // 需要触发对话的NPC

    void OnDestroy()
    {
        // 触发事件（编辑器绑定或代码调用）
        OnEnemyDestroyed?.Invoke();

        // 如果已手动指定NPC，直接调用其方法
        if (targetNPC != null)
        {
            targetNPC.StartSpecialDialogue();
        }
    }
}
