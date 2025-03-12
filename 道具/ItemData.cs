using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "Item Data")]
[System.Serializable]
public class ItemData : ScriptableObject
{
    public string itemName; // 道具名称
    public string description; // 道具描述
    public Sprite icon; // 道具图标

    // 道具效果类型
    public enum EffectType
    {
        RestoreHealth,    // 恢复血量
        ReduceDamage,     // 减少伤害
        AutoAnswer,       // 自动答对一题
        DamageEnemy,      // 直接对敌人造成伤害
        IncreaseAttack,   // 增加攻击力
    }

    public EffectType effectType; // 道具效果类型

    // 道具效果参数
    public int healthRestoreAmount; // 恢复血量值
    public int damageReductionAmount; // 减少伤害值
    public int enemyDamageAmount; // 对敌人造成的伤害值
    public int attackIncreaseAmount; // 攻击力增加值

    // 使用道具
    public string UseItem()
    {

        var backpackManager = BackpackManager.Instance;
        var currentItem = this;
        int quantity = backpackManager.GetItemQuantity(currentItem);

        if (quantity <= 0)
        {
            return "数量不足，使用失败";
        }

        backpackManager.AddItem(currentItem, -1); // 减少背包中道具数量

        var battleManager = GameObject.FindObjectOfType<BattleMechanismManager>();
        if (battleManager == null)
        {
            return "战斗管理器未找到，使用失败";
        }

        string effectDescription = description; // 使用道具描述

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
                effectDescription = "未知效果类型";
                break;
        }

        return effectDescription;
    }
}