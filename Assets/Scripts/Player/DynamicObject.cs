using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class DynamicObject : MonoBehaviour
{
    // 상속 메소드
    protected SpriteRenderer spriteRenderer;
    protected AudioSource audioSource;
    protected Animator animator;
    protected new CapsuleCollider2D collider;
    protected new Rigidbody2D rigidbody;

    protected float forward;
}

public class DynamicObjectExtension : DynamicObject
{
    public virtual void Awake()
    {
        this.audioSource = GetComponent<AudioSource>();
        this.rigidbody = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.collider = GetComponent<CapsuleCollider2D>();
    }

    protected void FlipX()
    {
        spriteRenderer.flipX = forward == -1;
    }
}
