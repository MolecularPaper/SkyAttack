using System.Collections;
using UnityEngine;


public class PlayerCTRL : PlayerExtension
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
        SetFriction();
         
        // 만약 플레이어가 밀리고 있는 상태라면, 여기서 리턴하여 조작을 제한함
        if (IsPushed)
            return;

        Move();
        Turn();
        Jump();
    }

    public void OnEnable() => key.OnEnabled();

    public void OnDisable() => key.OnDisable();

    // 플레이어의 이동을 처리함
    private void Move()
    {
        var input = key.Horizontal;

        if ((CanClimbAngle || input != GroundAnlgeDir) && !IsWall)
        {
            rigidbody.velocity = new Vector2(input * moveSpeed, rigidbody.velocity.y);
        }
    }

    public void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.DrawWireCube(WallCastPostion, WallCastSize);
        }
    }

    // 플레이어가 현재 점프 할 수 있는 상태인지 체크함
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

    // 플레이어의 점프를 처리함, 가변형 점프방식
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

        IsUp = rigidbody.velocity.y > 0f && !IsGround;
    }

    // 플레이어의 회전을 처리함
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
    
    // 플레이어가 데미지를 받았을경우 pushDir의 방향으로 pushSpeed만큼 밀음.
    // DamageDelay 동안은 플레이어의 조작이 제한됨
    public IEnumerator Push(Vector2 pushDir, float pushSpeed)
    {
        animator.SetTrigger("IsPushed");
        rigidbody.velocity = pushDir * pushSpeed;

        IsPushed = true;
        yield return new WaitForSeconds(damagedDelay);
        IsPushed = false;
    }
    
    //플레이어의 마찰력 계수를 설정해줌.
    // 플레이어가 움직이지 않거나 움직이지 못하는 경우는 마찰력 계수를 1로,
    // 플레이어가 움직이고 있다면 마찰력 계수를 0으로 설정해줌.
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

    // 플레이어의 Animator 파라미터값을 업데이트 해줌
    protected virtual void UpdateAnimator()
    {
        animator.SetBool("IsMove", IsMove && !IsWall);
        animator.SetBool("IsJump", IsUp);
        animator.SetBool("IsAir", !IsGround && rigidbody.velocity.y <= -0.3f);
    }

    // 플레이어의 발소리를 재생해줌. 이동할경우는 Animation에서 Event방식으로 호출됨.
    public void PlaySoundEffect(PlayerSoundEffectType type)
    {
        switch (type)
        {
            case PlayerSoundEffectType.Jump:
                audioSource.PlayOneShot(jumpSound[Random.Range(0, jumpSound.Length)]);
                break;
            case PlayerSoundEffectType.Land:
                audioSource.PlayOneShot(landSound[Random.Range(0, landSound.Length)]);
                break;
            case PlayerSoundEffectType.FootStep:
                if(IsGround)
                    audioSource.PlayOneShot(footStepSound[Random.Range(0, footStepSound.Length)]);
                break;
            default:
                throw new System.ArgumentOutOfRangeException(nameof(type), $"Not expected direction value: {type}");
        }
    }
}