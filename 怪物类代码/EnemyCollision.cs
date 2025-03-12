using UnityEngine;
using System.Collections.Generic;

public class EnemyCollision : MonoBehaviour
{
    public FadeManager fadeManager; // 引用 FadeManager 脚本

    // 支持的敌人标签列表
    private List<string> enemyTags = new List<string> { "enemy", "Boss" };

    private void OnTriggerEnter(Collider other)
    {
        // 检测是否碰撞到支持的敌人类型
        foreach (string tag in enemyTags)
        {
            if (other.CompareTag(tag))
            {
                // 记录玩家触碰敌人时的位置
                Vector3 playerPosition = transform.position;
                PlayerPrefs.SetFloat("PlayerX", playerPosition.x);
                PlayerPrefs.SetFloat("PlayerY", playerPosition.y);
                PlayerPrefs.SetFloat("PlayerZ", playerPosition.z);

                // 保存碰到的敌人名称作为标识
                PlayerPrefs.SetString("EnemyName", other.gameObject.name);

                // 触发 FadeManager 中的淡入方法
                if (fadeManager != null)
                {
                    fadeManager.StartFadeIn();
                }
                break; // 找到匹配的标签后退出循环
            }
        }
    }
}
