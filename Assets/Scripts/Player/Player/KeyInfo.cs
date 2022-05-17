using UnityEngine;
using UnityEngine.InputSystem;

public class KeyInfo
{
    private Player playerInput = null;

    private float horizontal;

    public bool jump;

    public float Horizontal
    {
        get
        {
            return horizontal;
        }
    }

    public void Awake()
    {
        playerInput = new Player();
        playerInput.Platformer.Move.performed += val => horizontal = val.ReadValue<float>();        
        playerInput.Platformer.Move.canceled += val => horizontal = val.ReadValue<float>();

        playerInput.Platformer.Jump.started += val => jump = true;
        playerInput.Platformer.Jump.canceled += val => jump = false;
    }

    public void OnEnabled() => playerInput.Enable();

    public void OnDisable() => playerInput.Disable();
}