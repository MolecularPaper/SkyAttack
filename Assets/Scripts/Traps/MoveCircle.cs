using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCircle : MoveBase
{
    [SerializeField] private Transform moveObject; // 움직이는 오브젝트
    [SerializeField] private float radius; // 중심 좌표로 부터의 거리
    [SerializeField] private float startAngle; // 움직이기 시작하는 각도
    [SerializeField] private bool isReverse; // 반시계 방향으로 회전하는가?

    private Quaternion moveObjectOriginRotation; // moveObject의 Rotation 기본값

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

    // 중심점 (이 오브젝트) 를 기준으로 moveObject를 공전시킴
    private void Rotate()
    {
        if (!IsMove)
            return;

        float rotateDir = Time.deltaTime * moveSpeed * (isReverse ? -1f : 1f);
        moveObject.RotateAround(transform.position, Vector3.forward, rotateDir);
        moveObject.rotation = moveObjectOriginRotation;
    }
}