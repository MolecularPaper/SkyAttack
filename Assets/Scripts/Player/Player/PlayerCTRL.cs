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
         
        // ���� �÷��̾ �и��� �ִ� ���¶��, ���⼭ �����Ͽ� ������ ������
        if (IsPushed)
            return;

        Move();
        Turn();
        Jump();
    }

    public void OnEnable() => key.OnEnabled();

    public void OnDisable() => key.OnDisable();

    // �÷��̾��� �̵��� ó����
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

    // �÷��̾ ���� ���� �� �� �ִ� �������� üũ��
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

    // �÷��̾��� ������ ó����, ������ �������
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

    // �÷��̾��� ȸ���� ó����
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
    
    // �÷��̾ �������� �޾������ pushDir�� �������� pushSpeed��ŭ ����.
    // DamageDelay ������ �÷��̾��� ������ ���ѵ�
    public IEnumerator Push(Vector2 pushDir, float pushSpeed)
    {
        animator.SetTrigger("IsPushed");
        rigidbody.velocity = pushDir * pushSpeed;

        IsPushed = true;
        yield return new WaitForSeconds(damagedDelay);
        IsPushed = false;
    }
    
    //�÷��̾��� ������ ����� ��������.
    // �÷��̾ �������� �ʰų� �������� ���ϴ� ���� ������ ����� 1��,
    // �÷��̾ �����̰� �ִٸ� ������ ����� 0���� ��������.
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

    // �÷��̾��� Animator �Ķ���Ͱ��� ������Ʈ ����
    protected virtual void UpdateAnimator()
    {
        animator.SetBool("IsMove", IsMove && !IsWall);
        animator.SetBool("IsJump", IsUp);
        animator.SetBool("IsAir", !IsGround && rigidbody.velocity.y <= -0.3f);
    }

    // �÷��̾��� �߼Ҹ��� �������. �̵��Ұ��� Animation���� Event������� ȣ���.
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