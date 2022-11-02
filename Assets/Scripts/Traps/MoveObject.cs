using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MoveBase
{
    [SerializeField] private Vector3[] moveLocalPoints; // 이동 좌표 목록 (로컬 기준)

    [Space(10)]
    [SerializeField] private Color pointColor;
    [SerializeField] protected bool isFlip;

    private SpriteRenderer spriteRenderer; // 이 오브젝트의 SpriteRendere
    private int moveIndex; // 현재 타켓 좌표의 배열의 인덱스

    private Vector3[] movePoints; // 이동 좌표 목록 (월드 기준)    
    public Vector3 CurrenMovePoint => movePoints[moveIndex]; // 현재 움직일 지점

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
        // 이동하지 않는 상태 일경우
        if (!IsMove)
        {
            // 플레이어가 접촉했을경우만 이동하며, 플레이어가 접촉해있지 않다면 기본 위치로 복귀함
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

    // 로컬 기준 이동 좌표 목록을 월드 기준으로 변경해서 movePoints 에 저장함
    private void SetMovePoints()
    {
        movePoints = new Vector3[moveLocalPoints.Length];
        for (int i = 0; i < moveLocalPoints.Length; i++)
        {
            movePoints[i] = transform.position + moveLocalPoints[i];
        }
    }

    // CurrenMovePoint의 좌표로 이동함
    private void MoveNext()
    {
        transform.position = Vector3.MoveTowards(transform.position, CurrenMovePoint, Time.deltaTime * moveSpeed);
    }

    // 기본 위치 (movePoint[0]) 좌표로 이동함
    private void MoveOrigin()
    {
        if(Vector3.Distance(movePoints[0], transform.position) != 0.0f)
            transform.position = Vector3.MoveTowards(transform.position, movePoints[0], Time.deltaTime * moveSpeed);
    }

    // 오브젝트의 Sprite를 반전시킴
    private void Flip()
    {
        // 반전시키지 않는 오브젝트일경우 리턴한다.
        if (!isFlip)
            return;

        // 이동 방향을 계산한다.
        Vector3 dir = transform.position - CurrenMovePoint;

        // 왼쪽으로 이동할경우 반전, 아닐경우는 원래 상태로 되돌린다.
        if (dir.x > 0)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;
    }

    // MoveIndex 증가시킴
    private void NextMoveIndex()
    {
        moveIndex++;

        // MoveIndex가 MovePoint 배열의 길이와 크거나 같을경우 MoveIndex를 0으로 설정함
        if (moveIndex >= movePoints.Length)
        {
            moveIndex = 0;

            // 플레이어가 접촉해야만 이동할경우, 이동하지 않는 상태로 변경함
            if (moveOnPlayer)
                IsMove = false;
        }
    }

    // 플레이어가 이 오브젝트의 Postion 변경의 영향을 받게 하기 위하여,
    // 플레이어와 접촉시 이 오브젝트를 플레이어의 부모 Transform으로 만들어줌
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
