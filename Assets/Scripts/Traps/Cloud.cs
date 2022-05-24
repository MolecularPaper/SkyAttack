using UnityEngine;

public class Cloud : MonoBehaviour
{
    [SerializeField] private float extinctionSpeed;

    private SpriteRenderer spriteRenderer;
    private new Collider2D collider;
    private bool onPlayer = false;

    private Color originColor;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        originColor = spriteRenderer.color;
    }

    protected virtual void Update()
    {
        if (onPlayer)
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

    protected virtual void OnCollisionEnter2D(Collision2D collision) => onPlayer = true;

    protected virtual void OnCollisionExit2D(Collision2D collision)
    {
        onPlayer = false;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision) => onPlayer = true;

    protected virtual void OnTriggerExit2D(Collider2D collision) => onPlayer = false;
}
