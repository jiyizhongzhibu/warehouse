using UnityEngine;

public class EnemyPatrolFixedDistance : MonoBehaviour
{
    // �ƶ��ٶ�
    public float moveSpeed = 3f;
    // Ѳ�ߵĹ̶����루��������Ϊ 5 �ף�
    public float patrolDistance = 10f;

    // ��ʼλ��
    private Vector3 startPosition;
    // ��ǰ�ƶ�����1 ��ʾ��ǰ��-1 ��ʾ���
    private int direction = 1;
    // �Ѿ��ƶ��ľ���
    private float distanceMoved = 0f;

    // ���������
    private Animator animator;

    void Start()
    {
        // ��¼С�ֵĳ�ʼλ��
        startPosition = transform.position;

        // ��ȡ���������
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // ���㵱ǰ֡���ƶ�����
        float currentMoveDistance = moveSpeed * Time.deltaTime;
        // ���㵱ǰ֡���ƶ�����
        Vector3 moveVector = transform.forward * currentMoveDistance * direction;

        // �����Ѿ��ƶ��ľ���
        distanceMoved += currentMoveDistance;

        // �ƶ�С��
        transform.position += moveVector;

        // ����Ƿ�ﵽѲ�߾���
        if (distanceMoved >= patrolDistance)
        {
            //// �ı��ƶ�����
            //direction *= -1;
            // �����Ѿ��ƶ��ľ���
            distanceMoved = 0f;

            // ��ת���� 180 ��
            transform.Rotate(0f, 180f, 0f);
        }

        // ���¶������� Speed ����
        if (animator != null)
        {
            animator.SetFloat("Speed", 1f);
        }
    }
}