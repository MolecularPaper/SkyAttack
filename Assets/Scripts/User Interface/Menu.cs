using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    void Awake()
    {
        gameObject.SetActive(false);
        PlayerCTRL playerCTRL = GameObject.FindWithTag("Player").GetComponent<PlayerCTRL>();
        playerCTRL.key.AddMenuKey(this.gameObject);
    }
}
