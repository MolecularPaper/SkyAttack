using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class LoopTrap : MonoBehaviour
{
    [SerializeField] private UnityEvent triggerEvent;
    [SerializeField] private float triggerDelay;

    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(Loop());
    }

    private IEnumerator Loop()
    {
        WaitForSeconds triggerDelay = new WaitForSeconds(this.triggerDelay);

        while (true)
        {
            animator.SetTrigger("Trigger");
            yield return triggerDelay;
        }
    }

    public void Trigger() => triggerEvent.Invoke();
}
