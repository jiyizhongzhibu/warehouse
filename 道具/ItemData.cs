using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "Item Data")]
[System.Serializable]
public class ItemData : ScriptableObject
{
    public string itemName; // ��������
    public string description; // ��������
    public Sprite icon; // ����ͼ��

    // ����Ч������
    public enum EffectType
    {
        RestoreHealth,    // �ָ�Ѫ��
        ReduceDamage,     // �����˺�
        AutoAnswer,       // �Զ����һ��
        DamageEnemy,      // ֱ�ӶԵ�������˺�
        IncreaseAttack,   // ���ӹ�����
    }

    public EffectType effectType; // ����Ч������

    // ����Ч������
    public int healthRestoreAmount; // �ָ�Ѫ��ֵ
    public int damageReductionAmount; // �����˺�ֵ
    public int enemyDamageAmount; // �Ե�����ɵ��˺�ֵ
    public int attackIncreaseAmount; // ����������ֵ

    // ʹ�õ���
    public string UseItem()
    {

        var backpackManager = BackpackManager.Instance;
        var currentItem = this;
        int quantity = backpackManager.GetItemQuantity(currentItem);

        if (quantity <= 0)
        {
            return "�������㣬ʹ��ʧ��";
        }

        backpackManager.AddItem(currentItem, -1); // ���ٱ����е�������

        var battleManager = GameObject.FindObjectOfType<BattleMechanismManager>();
        if (battleManager == null)
        {
            return "ս��������δ�ҵ���ʹ��ʧ��";
        }

        string effectDescription = description; // ʹ�õ�������

        switch (effectType)
        {
            case EffectType.RestoreHealth:
                battleManager.RestorePlayerHealth(healthRestoreAmount);
                break;
            case EffectType.ReduceDamage:
                battleManager.SetDamageReduction(damageReductionAmount);
                break;
            case EffectType.AutoAnswer:
                battleManager.EnableAutoAnswer();
                break;
            case EffectType.DamageEnemy:
                battleManager.DamageEnemy(enemyDamageAmount);
                break;
            case EffectType.IncreaseAttack:
                battleManager.IncreaseAttack(attackIncreaseAmount);
                break;
            default:
                effectDescription = "δ֪Ч������";
                break;
        }

        return effectDescription;
    }
}