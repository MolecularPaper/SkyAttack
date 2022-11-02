using UnityEngine;

[RequireComponent(typeof(CheckOnPlayer))]
public class Cloud : MonoBehaviour
{
    [SerializeField] private float extinctionSpeed;

    private CheckOnPlayer checkOnPlayer; // �÷��̾ ���� �浹�� �ִ��� Ȯ���ϴ� ������Ʈ
    private SpriteRenderer spriteRenderer; // �� ������Ʈ�� SpriteRenderer
    private new Collider2D collider; // �� ������Ʈ�� �ݶ��̴�
    private Color tempColor;

    protected virtual void Awake()
    {
        checkOnPlayer = GetComponent<CheckOnPlayer>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        tempColor = spriteRenderer.color;
    }

    protected virtual void Update()
    {
        // �÷��̾ ���� �ִٸ� SpritreRender�� Color�� Alpha ���� 0���� ����, ���ٸ� 1�� �ø�
        if (checkOnPlayer.OnPlayer) {
            tempColor.a = Mathf.MoveTowards(tempColor.a, 0, extinctionSpeed * Time.deltaTime);
            spriteRenderer.color = tempColor;

            // Alpha���� 0 �̶�� Collider �浹 ��Ȱ��ȭ
            if (tempColor.a == 0)
                collider.isTrigger = true;
        }
        else {

            tempColor.a = Mathf.MoveTowards(tempColor.a, 1, extinctionSpeed * Time.deltaTime);
            spriteRenderer.color = tempColor;

            // Alpha���� 1 �̶�� Collider �浹 Ȱ��ȭ
            if (tempColor.a == 1)
                collider.isTrigger = false;
        }
    }
}