using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCircle : MoveBase
{
    [SerializeField] private Transform moveObject; // �����̴� ������Ʈ
    [SerializeField] private float radius; // �߽� ��ǥ�� ������ �Ÿ�
    [SerializeField] private float startAngle; // �����̱� �����ϴ� ����
    [SerializeField] private bool isReverse; // �ݽð� �������� ȸ���ϴ°�?

    private Quaternion moveObjectOriginRotation; // moveObject�� Rotation �⺻��

    public override void Awake()
    {
        base.Awake();

        moveObject.localPosition = new Vector3(0, radius, 0);
        moveObjectOriginRotation = moveObject.rotation;

        transform.rotation = Quaternion.Euler(0, 0, startAngle);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position + new Vector3(0, radius, 0), 0.3f);
    }

    void Update()
    {
        Rotate();
    }

    // �߽��� (�� ������Ʈ) �� �������� moveObject�� ������Ŵ
    private void Rotate()
    {
        if (!IsMove)
            return;

        float rotateDir = Time.deltaTime * moveSpeed * (isReverse ? -1f : 1f);
        moveObject.RotateAround(transform.position, Vector3.forward, rotateDir);
        moveObject.rotation = moveObjectOriginRotation;
    }
}