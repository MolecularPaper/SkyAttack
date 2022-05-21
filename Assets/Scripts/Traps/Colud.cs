using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine;

public class Colud : MonoBehaviour
{
    [SerializeField] private float extinctionSpeed;

    private SpriteRenderer spriteRenderer;
    private new Collider2D collider;
    private bool onPlayer = false;

    private Color originColor;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        originColor = spriteRenderer.color;
    }

    void Update()
    {
        if (onPlayer)
        {
            originColor.a = Mathf.MoveTowards(originColor.a, 0, extinctionSpeed * Time.deltaTime);
            spriteRenderer.color = originColor;

            if (originColor.a == 0)
            {
                collider.enabled = false;
            }
        }
        else
        {
            originColor.a = Mathf.MoveTowards(originColor.a, 1, extinctionSpeed * Time.deltaTime);
            spriteRenderer.color = originColor;

            if (originColor.a == 1)
            {
                collider.enabled = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) => onPlayer = true;

    private void OnCollisionExit2D(Collision2D collision)
    {
        Task.Run(async () => {
            await Task.Delay(500);
            onPlayer = false;
        });
    }
}
