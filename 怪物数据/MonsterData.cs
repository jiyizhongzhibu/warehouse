using UnityEngine;

[CreateAssetMenu(fileName = "NewMonsterData", menuName = "Monster Data")]
public class MonsterData : ScriptableObject
{
    public string monsterName; // 怪物名称
    public GameObject prefab;  // 怪物预制体
    public float maxHealth;    // 最大血量
    public float damagePerCorrectAnswer; // 答对题扣血值
    public float damagePerWrongAnswer;   // 答错题扣血值
}