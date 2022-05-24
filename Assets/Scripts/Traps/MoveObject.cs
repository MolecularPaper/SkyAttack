using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    [SerializeField] private Vector3[] moveLocalPoints;
    [SerializeField] private bool moveOnAwake;
    [SerializeField] private bool isFlip;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Color pointColor;

    private SpriteRenderer spriteRenderer;

    private Vector3[] movePoints;
    private bool isMove;
    private int moveIndex;

    public Vector3 CurrenPoint
    {
        get
        {
            return movePoints[moveIndex];
        }
    }

    public bool IsMove
    {
        get => isMove;
        set => isMove = value;
    }

    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        isMove = moveOnAwake;
        moveIndex = 1;

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
        if (!isMove)
            return;

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
            moveIndex = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            collision.transform.parent = this.transform;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            collision.transform.parent = null;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            collision.transform.parent = null;
    }
}
