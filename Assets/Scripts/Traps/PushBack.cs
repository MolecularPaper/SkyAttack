using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CheckOnPlayer))]
public class PushBack : MonoBehaviour
{
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
            StartCoroutine(playerCTRL.Damaged());
        }
    }
}
