using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CheckOnPlayer))]
public class PushBack : MonoBehaviour
{
    [SerializeField] private Vector2[] pushDir;
    [SerializeField] private float pushSpeed;

    private CheckOnPlayer checkOnPlayer;
    private PlayerCTRL playerCTRL;

    public void Awake()
    {
        playerCTRL = GameObject.FindWithTag("Player").GetComponent<PlayerCTRL>();
        checkOnPlayer = GetComponent<CheckOnPlayer>();
    }

    public void Push()
    {
        if (checkOnPlayer.OnPlayer)
        {
            Vector2 pushDir = this.pushDir[Random.Range(0, this.pushDir.Length)];
            pushDir = transform.TransformDirection(pushDir);
            StartCoroutine(playerCTRL.Push(pushDir, pushSpeed));
        }
    }
}
