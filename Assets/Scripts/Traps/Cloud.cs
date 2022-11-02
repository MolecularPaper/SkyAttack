using UnityEngine;

[RequireComponent(typeof(CheckOnPlayer))]
public class Cloud : MonoBehaviour
{
    [SerializeField] private float extinctionSpeed;

    private CheckOnPlayer checkOnPlayer; // 플레이어가 현재 충돌해 있는지 확인하는 컴포넌트
    private SpriteRenderer spriteRenderer; // 이 오브젝트의 SpriteRenderer
    private new Collider2D collider; // 이 오브젝트의 콜라이더
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
        // 플레이어가 위에 있다면 SpritreRender의 Color의 Alpha 값을 0으로 줄임, 없다면 1로 늘림
        if (checkOnPlayer.OnPlayer) {
            tempColor.a = Mathf.MoveTowards(tempColor.a, 0, extinctionSpeed * Time.deltaTime);
            spriteRenderer.color = tempColor;

            // Alpha값이 0 이라면 Collider 충돌 비활성화
            if (tempColor.a == 0)
                collider.isTrigger = true;
        }
        else {

            tempColor.a = Mathf.MoveTowards(tempColor.a, 1, extinctionSpeed * Time.deltaTime);
            spriteRenderer.color = tempColor;

            // Alpha값이 1 이라면 Collider 충돌 활성화
            if (tempColor.a == 1)
                collider.isTrigger = false;
        }
    }
}