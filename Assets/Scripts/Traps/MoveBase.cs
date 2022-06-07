using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CheckOnPlayer))]
public class MoveBase : MonoBehaviour
{
    [SerializeField] protected bool moveOnPlayer;
    [SerializeField] protected float moveSpeed;

    [HideInInspector] public bool IsMove { get; set; }

    protected CheckOnPlayer checkOnPlayer;
}

public class MoveExtension : MoveBase
{
    public virtual void Awake()
    {
        checkOnPlayer = GetComponent<CheckOnPlayer>();
        IsMove = !moveOnPlayer;
    }
}