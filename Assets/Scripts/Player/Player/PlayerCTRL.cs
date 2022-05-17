using System.Threading.Tasks;
using UnityEngine;


public class PlayerCTRL : PlayerTirggers
{
    public override void Awake()
    {
        base.Awake();
        key.Awake();

        forward = spriteRenderer.flipX ? -1 : 1;
    }

    public virtual void Update()
    {
        CheckJump();
        UpdateAnimator();
    }

    public virtual void FixedUpdate()
    {
        Move();
        Turn();
        Jump();
        SetFriction();
    }

    public void OnEnable() => key.OnEnabled();

    public void OnDisable() => key.OnDisable();

    private void Move()
    {
        var dir = new Vector2(key.Horizontal, 0);
        var input = key.Horizontal;

        if ((CanClimbAngle || dir.x != GroundAnlgeDir) && !IsWall)
        {
            rigidbody.velocity = new Vector2(input * moveSpeed, rigidbody.velocity.y);
        }
    }

    private void CheckJump()
    {
        if (rigidbody.velocity.y <= -0.1f || TopCast.collider != null)
        {
            IsJump = false;
            key.jump = false;
            return;
        }

        if(key.jump && IsGround)
        {
            IsJump = true;
            return;
        }

        if(!key.jump)
        {
            IsJump = false;
            return;
        }
    }

    private void Jump()
    {
        if (IsJump && rigidbody.velocity.y <= jumpSpeed)
        {
            rigidbody.AddRelativeForce(Vector2.up * jumpAccel, ForceMode2D.Impulse);
        }
        else
        {
            IsJump = false;
        }

        IsUp = rigidbody.velocity.y > 0.1f;
    }

    private void Turn()
    {
        float forward = key.Horizontal;
        if (forward != 0)
        {
            if (this.forward != forward)
            {
                IsTurn = true;
                this.forward = forward;
                animator.SetTrigger("IsTurn");
            }

            if (!IsGround)
            {
                FlipX();
                IsTurn = false;
            }
        }
    }

    private void SetFriction()
    {
        if (CanClimbAngle && !IsMove)
        {
            collider.sharedMaterial.friction = 1.0f;
        }
        else
        {
            collider.sharedMaterial.friction = 0.0f;
        }

        collider.sharedMaterial.bounciness = 0.0f;
    }

    protected virtual void UpdateAnimator()
    {
        animator.SetBool("IsMove", IsMove && !IsWall);
        animator.SetBool("IsJump", IsUp);
        animator.SetBool("IsGround", IsGround);
    }
}