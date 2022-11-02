using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MoveBase
{
    [SerializeField] private Vector3[] moveLocalPoints; // �̵� ��ǥ ��� (���� ����)

    [Space(10)]
    [SerializeField] private Color pointColor;
    [SerializeField] protected bool isFlip;

    private SpriteRenderer spriteRenderer; // �� ������Ʈ�� SpriteRendere
    private int moveIndex; // ���� Ÿ�� ��ǥ�� �迭�� �ε���

    private Vector3[] movePoints; // �̵� ��ǥ ��� (���� ����)    
    public Vector3 CurrenMovePoint => movePoints[moveIndex]; // ���� ������ ����

    public override void Awake()
    {
        base.Awake();

        spriteRenderer = GetComponent<SpriteRenderer>();

        SetMovePoints();
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = pointColor;

        if (Application.isPlaying)
        {
            foreach (var point in movePoints)
                Gizmos.DrawSphere(point, 0.3f);
        }
        else
        {
            foreach (var point in moveLocalPoints)
                Gizmos.DrawSphere(transform.position + point, 0.3f);
        }
    }

    public void Update()
    {
        // �̵����� �ʴ� ���� �ϰ��
        if (!IsMove)
        {
            // �÷��̾ ����������츸 �̵��ϸ�, �÷��̾ ���������� �ʴٸ� �⺻ ��ġ�� ������
            if(moveOnPlayer && !checkOnPlayer.OnPlayer)
                MoveOrigin();
            return;
        }

        if (Vector3.Distance(CurrenMovePoint, transform.position) != 0.0f)
        {
            MoveNext();
            return;
        }

        NextMoveIndex();
        Flip();
    }

    // ���� ���� �̵� ��ǥ ����� ���� �������� �����ؼ� movePoints �� ������
    private void SetMovePoints()
    {
        movePoints = new Vector3[moveLocalPoints.Length];
        for (int i = 0; i < moveLocalPoints.Length; i++)
        {
            movePoints[i] = transform.position + moveLocalPoints[i];
        }
    }

    // CurrenMovePoint�� ��ǥ�� �̵���
    private void MoveNext()
    {
        transform.position = Vector3.MoveTowards(transform.position, CurrenMovePoint, Time.deltaTime * moveSpeed);
    }

    // �⺻ ��ġ (movePoint[0]) ��ǥ�� �̵���
    private void MoveOrigin()
    {
        if(Vector3.Distance(movePoints[0], transform.position) != 0.0f)
            transform.position = Vector3.MoveTowards(transform.position, movePoints[0], Time.deltaTime * moveSpeed);
    }

    // ������Ʈ�� Sprite�� ������Ŵ
    private void Flip()
    {
        // ������Ű�� �ʴ� ������Ʈ�ϰ�� �����Ѵ�.
        if (!isFlip)
            return;

        // �̵� ������ ����Ѵ�.
        Vector3 dir = transform.position - CurrenMovePoint;

        // �������� �̵��Ұ�� ����, �ƴҰ��� ���� ���·� �ǵ�����.
        if (dir.x > 0)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;
    }

    // MoveIndex ������Ŵ
    private void NextMoveIndex()
    {
        moveIndex++;

        // MoveIndex�� MovePoint �迭�� ���̿� ũ�ų� ������� MoveIndex�� 0���� ������
        if (moveIndex >= movePoints.Length)
        {
            moveIndex = 0;

            // �÷��̾ �����ؾ߸� �̵��Ұ��, �̵����� �ʴ� ���·� ������
            if (moveOnPlayer)
                IsMove = false;
        }
    }

    // �÷��̾ �� ������Ʈ�� Postion ������ ������ �ް� �ϱ� ���Ͽ�,
    // �÷��̾�� ���˽� �� ������Ʈ�� �÷��̾��� �θ� Transform���� �������
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            collision.transform.parent = this.transform;
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            collision.transform.parent = null;
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            collision.transform.parent = null;
    }
}
