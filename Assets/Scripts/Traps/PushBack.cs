using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CheckOnPlayer))]
public class PushBack : MonoBehaviour
{
    [SerializeField] private float triggerDelay;

    private CheckOnPlayer checkOnPlayer;
    private PlayerCTRL playerCTRL;
    private Animator animator;

    public void Awake()
    {
        playerCTRL = GameObject.FindWithTag("Player").GetComponent<PlayerCTRL>();
        checkOnPlayer = GetComponent<CheckOnPlayer>();
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
    
    public void Push()
    {
        if (checkOnPlayer.onPlayer)
        {
            playerCTRL.Damaged();
        }
    }
}
