using UnityEngine;
public class KeyInfo
{
    private PlayerInput playerInput = null; // �Է� �ý���

    public bool jump; // �÷��̾ ����Ű�� �����°�?

    private float horizontal; // �÷��̾� �̵� ���Ⱚ
    public float Horizontal
    {
        get
        {
            return horizontal;
        }
    }

    // �Է� �ý��� �ʱ�ȭ ��, �Է� �ý��ۿ� �Է� �̺�Ʈ ���
    public void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.Platformer.Move.performed += val => horizontal = val.ReadValue<float>();        
        playerInput.Platformer.Move.canceled += val => horizontal = val.ReadValue<float>();

        playerInput.Platformer.Jump.started += val => jump = true;
        playerInput.Platformer.Jump.canceled += val => jump = false;
    }

    // �޴� Ű �Է� �̺�Ʈ ���
    public void AddMenuKey(GameObject menu) => playerInput.Platformer.Menu.started += val => menu.SetActive(!menu.activeSelf);

    // �Է� �ý��� Ȱ��ȭ
    public void OnEnabled() => playerInput.Enable();

    // �Է� �ý��� ��Ȱ��ȭ
    public void OnDisable() => playerInput.Disable();
}