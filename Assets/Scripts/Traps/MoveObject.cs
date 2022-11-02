using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MoveBase
{
    [SerializeField] private Vector3[] moveLocalPoints;

    [Space(10)]
    [SerializeField] private Color pointColor;
    [SerializeField] protected bool isFlip;

    private SpriteRenderer spriteRenderer;
    private Vector3[] movePoints;
    private int moveIndex;

    public Vector3 CurrenPoint
    {
        get
        {
            return movePoints[moveIndex];
        }
    }

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
        if (!IsMove)
        {
            if(moveOnPlayer && !checkOnPlayer.OnPlayer && Vector3.Distance(movePoints[0], transform.position) != 0.0f)
                MoveOrigin();
            return;
        }

        if (Vector3.Distance(CurrenPoint, transform.position) != 0.0f)
        {
            MoveNext();
            return;
        }

        NextMoveIndex();
        Flip();
    }

    private void SetMovePoints()
    {
        movePoints = new Vector3[moveLocalPoints.Length];
        for (int i = 0; i < moveLocalPoints.Length; i++)
        {
            movePoints[i] = transform.position + moveLocalPoints[i];
        }
    }

    private void MoveNext()
    {
        transform.position = Vector3.MoveTowards(transform.position, CurrenPoint, Time.deltaTime * moveSpeed);
    }

    private void MoveOrigin()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoints[0], Time.deltaTime * moveSpeed);
    }

    private void Flip()
    {
        if (!isFlip)
            return;

        Vector3 dir = transform.position - CurrenPoint;

        if (dir.x > 0)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;
    }

    private void NextMoveIndex()
    {
        moveIndex++;

        if (moveIndex == movePoints.Length)
        {
            moveIndex = 0;

            if (moveOnPlayer)
                IsMove = false;
        }
    }

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
