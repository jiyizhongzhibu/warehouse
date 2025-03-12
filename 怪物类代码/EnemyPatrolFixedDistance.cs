using UnityEngine;

public class EnemyPatrolFixedDistance : MonoBehaviour
{
    // 移动速度
    public float moveSpeed = 3f;
    // 巡逻的固定距离（这里设置为 5 米）
    public float patrolDistance = 10f;

    // 初始位置
    private Vector3 startPosition;
    // 当前移动方向，1 表示向前，-1 表示向后
    private int direction = 1;
    // 已经移动的距离
    private float distanceMoved = 0f;

    // 动画器组件
    private Animator animator;

    void Start()
    {
        // 记录小怪的初始位置
        startPosition = transform.position;

        // 获取动画器组件
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // 计算当前帧的移动距离
        float currentMoveDistance = moveSpeed * Time.deltaTime;
        // 计算当前帧的移动向量
        Vector3 moveVector = transform.forward * currentMoveDistance * direction;

        // 更新已经移动的距离
        distanceMoved += currentMoveDistance;

        // 移动小怪
        transform.position += moveVector;

        // 检查是否达到巡逻距离
        if (distanceMoved >= patrolDistance)
        {
            //// 改变移动方向
            //direction *= -1;
            // 重置已经移动的距离
            distanceMoved = 0f;

            // 旋转敌人 180 度
            transform.Rotate(0f, 180f, 0f);
        }

        // 更新动画器的 Speed 参数
        if (animator != null)
        {
            animator.SetFloat("Speed", 1f);
        }
    }
}