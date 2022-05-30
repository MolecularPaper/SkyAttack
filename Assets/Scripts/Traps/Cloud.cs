using UnityEngine;

[RequireComponent(typeof(CheckOnPlayer))]
public class Cloud : MonoBehaviour
{
    [SerializeField] private float extinctionSpeed;

    private CheckOnPlayer checkOnPlayer;
    private SpriteRenderer spriteRenderer;
    private new Collider2D collider;

    private Color originColor;

    protected virtual void Awake()
    {
        checkOnPlayer = GetComponent<CheckOnPlayer>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        originColor = spriteRenderer.color;
    }

    protected virtual void Update()
    {
        if (checkOnPlayer.onPlayer)
        {
            originColor.a = Mathf.MoveTowards(originColor.a, 0, extinctionSpeed * Time.deltaTime);
            spriteRenderer.color = originColor;

            if (originColor.a == 0)
            {
                collider.isTrigger = true;
            }
        }
        else
        {
            originColor.a = Mathf.MoveTowards(originColor.a, 1, extinctionSpeed * Time.deltaTime);
            spriteRenderer.color = originColor;

            if (originColor.a == 1)
            {
                collider.isTrigger = false;
            }
        }
    }
}
