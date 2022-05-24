using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    [SerializeField] private Vector3[] movePoints;
    [SerializeField] private bool moveOnAwake;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Color pointColor;

    private bool isMove;
    private int moveIndex;

    public bool IsMove
    {
        get => isMove;
        set => isMove = value;
    }

    public void Awake()
    {
        isMove = moveOnAwake;
        moveIndex = 1;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = pointColor;

        foreach (var point in movePoints)
        {
            Gizmos.DrawSphere(point, 0.3f);
        }
    }

    public void Update()
    {
        if (!isMove)
            return;

        if (Vector3.Distance(movePoints[moveIndex], transform.position) != 0.0f)
        {
            MoveNext();
            return;
        }

        NextMoveIndex();
    }

    private void MoveNext()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoints[moveIndex], Time.deltaTime * moveSpeed);
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
