using UnityEngine;

[CreateAssetMenu(fileName = "NewMonsterData", menuName = "Monster Data")]
public class MonsterData : ScriptableObject
{
    public string monsterName; // ��������
    public GameObject prefab;  // ����Ԥ����
    public float maxHealth;    // ���Ѫ��
    public float damagePerCorrectAnswer; // ������Ѫֵ
    public float damagePerWrongAnswer;   // ������Ѫֵ
}