using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckOnPlayer : MonoBehaviour
{
    public bool onPlayer = false;

    protected virtual void OnCollisionEnter2D(Collision2D collision) => onPlayer = true;

    protected virtual void OnCollisionExit2D(Collision2D collision)
    {
        onPlayer = false;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision) => onPlayer = true;

    protected virtual void OnTriggerExit2D(Collider2D collision) => onPlayer = false;
}
