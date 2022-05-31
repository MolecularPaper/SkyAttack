using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCircle : MoveExtension
{
    [SerializeField] private Transform moveObject;
    [SerializeField] private float radius;
    [SerializeField] private float startAngle;
    [SerializeField] private bool isReverse;

    public override void Awake()
    {
        base.Awake();

        moveObject.localPosition = new Vector3(0, radius, 0);
        transform.rotation = Quaternion.Euler(0, 0, startAngle);
    }

    void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        if (!isMove)
            return;

        Vector3 rotDir = new Vector3(0, 0, isReverse ? -1f : 1f);
        transform.Rotate(Time.deltaTime * moveSpeed * rotDir);
    }
}
