using UnityEngine;
public class KeyInfo
{
    private PlayerInput playerInput = null; // 입력 시스템

    public bool jump; // 플레이어가 점프키를 눌렀는가?

    private float horizontal; // 플레이어 이동 방향값
    public float Horizontal
    {
        get
        {
            return horizontal;
        }
    }

    // 입력 시스템 초기화 및, 입력 시스템에 입력 이벤트 등록
    public void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.Platformer.Move.performed += val => horizontal = val.ReadValue<float>();        
        playerInput.Platformer.Move.canceled += val => horizontal = val.ReadValue<float>();

        playerInput.Platformer.Jump.started += val => jump = true;
        playerInput.Platformer.Jump.canceled += val => jump = false;
    }

    // 메뉴 키 입력 이벤트 등록
    public void AddMenuKey(GameObject menu) => playerInput.Platformer.Menu.started += val => menu.SetActive(!menu.activeSelf);

    // 입력 시스템 활성화
    public void OnEnabled() => playerInput.Enable();

    // 입력 시스템 비활성화
    public void OnDisable() => playerInput.Disable();
}