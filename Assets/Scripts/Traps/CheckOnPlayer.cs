using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class CheckOnPlayer : MonoBehaviour
{
    [HideInInspector] private bool onPlayer = false;
    [HideInInspector] public UnityAction valueChangeAction;

    [SerializeField] private UnityEvent playerEnterEvnet;
    [SerializeField] private UnityEvent playerExitEvnet;
    [SerializeField] private UnityEvent playerTriggerEnterEvnet;
    [SerializeField] private UnityEvent playerTriggerExitEvnet;

    public bool OnPlayer
    {
        get => onPlayer;
        private set
        {
            onPlayer = value;
            
            if(valueChangeAction != null)
                valueChangeAction.Invoke();
        }
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        OnPlayer = true;
        playerEnterEvnet.Invoke();
    }

    public virtual void OnCollisionExit2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        OnPlayer = false;
        playerExitEvnet.Invoke();
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        OnPlayer = true;
        playerTriggerEnterEvnet.Invoke();
    }

    public virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        OnPlayer = false;
        playerTriggerExitEvnet.Invoke();
    }
}
