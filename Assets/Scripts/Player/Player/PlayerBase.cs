using UnityEngine;

[System.Serializable]
public enum PlayerSoundEffectType
{
    Jump,
    Land,
    FootStep,
}

public class PlayerBase : DynamicObjectExtension
{
    [Space(10)]
    [SerializeField] protected float moveSpeed; // 플레이어 이동 속도
    [SerializeField] protected float jumpSpeed; // 플레이어 점프 속도
    [SerializeField] protected float jumpAccel; // 플레이어 점프 가속 속도 
    [SerializeField] protected float maxClimbAngle; // 플레이이어가 이동 가능한 최대 각도
    [SerializeField] protected float damagedDelay; // 데미지를 받았을때 프리징 되는 시간

    [Space(10)]
    [SerializeField] protected AudioClip[] jumpSound; // 플레이어가 점프했을경우 재생되는 효과음
    [SerializeField] protected AudioClip[] landSound; // 플레이어가 착지했을경우 재생되는 효과음
    [SerializeField] protected AudioClip[] footStepSound; // 플레이어 이동시 재생되는 발소리 효과음

    [HideInInspector] public KeyInfo key = new KeyInfo(); // 플레이어 입력 처리를 담당하는 클래스

    protected bool IsUp; // 플레이어가 상승 중인가? (점프 시작 ~ 하강 전까지)
    protected bool IsTurn; // 플레이어가 이동 방향을 바꾸었는가?
    protected bool IsJump; // 플레이어가 점프 중인가?
    protected bool IsPushed; // 플레어가 밀리는 중인가? (데미지를 받았을경우)
}

public class PlayerExtension : PlayerBase
{
    // GroundCast의 원점 좌표
    protected Vector2 GroundCheckPostion
    {
        get
        {
            return new Vector2(transform.position.x, transform.position.y + collider.size.x - 0.2f);
        }
    }

    // 플레이어가 지면에 닿아있는 확인하기 위한 RayCast
    protected RaycastHit2D GroundCast
    {
        get
        {
            int layerMask = (-1) - (1 << LayerMask.NameToLayer("Player"));
            return Physics2D.CircleCast(GroundCheckPostion, collider.size.x / 2f, Vector2.down, collider.size.y, layerMask);
        }
    }

    // WallCast 의 원점 좌표
    protected Vector2 WallCastPostion
    {
        get
        {
            return transform.position + new Vector3(0.02f * forward, collider.size.y / 2f);
        }
    }


    // WallCast Box 의 크기
    protected Vector2 WallCastSize
    {
        get
        {
            return new Vector2(collider.size.x, collider.size.y - 0.2f);
        }
    }

    // WallCast 의 방향 (플레이어가 보고 있는 방향)
    protected Vector2 WallCastDirection
    {
        get
        {
            return Vector3.right * forward;
        }
    }

    // 플레이어가 벽면에 닿아있는지 확인하기 위한 RayCast
    protected RaycastHit2D WallCast
    {
        get
        { 
            int layerMask = 1 << LayerMask.NameToLayer("Terrain");
            return Physics2D.CapsuleCast(WallCastPostion, WallCastSize, CapsuleDirection2D.Vertical, 0.0f, WallCastDirection, collider.size.x / 2f, layerMask);
        }
    }

    // TopCast 의 원점 좌표
    protected Vector2 TopCastPostion
    {
        get
        {
            return transform.position + new Vector3(0f, collider.size.y + 0.2f);
        }
    }

    // TopCast Box 의 크기
    protected Vector2 TopCastSize
    {
        get
        {
            return new Vector2(collider.size.x - 0.02f, 0.1f);
        }
    }

    // 플레이어 위에 물체가 있는지 확인하기 위한 RayCast
    protected RaycastHit2D TopCast
    {
        get
        {
            int layerMask = 1 << LayerMask.NameToLayer("Terrain");
            return Physics2D.BoxCast(TopCastPostion, TopCastSize, 0, transform.up, 0.01f, layerMask: layerMask);
        }
    }

    // 플레이어가 현재 움직이고 있는가?
    protected bool IsMove
    {
        get
        {
            return key.Horizontal != 0;
        }
    }

    // 플레이어가 현재 벽면에 닿아있가?
    protected bool IsWall
    {
        get
        {
            return WallCast.collider != null;
        }
    }

    // 플레이어가 현재 지면과 닿아있는가?
    protected bool IsGround
    {
        get
        {
            return GroundCast.collider != null;
        }
    }

    // 플레이어가 닿아있는 지면의 각도
    protected float GroundAngle
    {
        get
        {
            return Vector2.Angle(GroundCast.normal, Vector2.up);
        }
    }

    // X축을 기준으로 현재 지면의 노말 벡터가 향하고 있는 방향
    protected float GroundAnlgeDir
    {
        get
        {
            return GroundCast.normal.x < 0 ? 1 : -1;
        }
    }

    // 플레이어가 닿아있는 지면의 각도가, 이동할 수 있는 지면의 최대 각도보다 작은가?
    protected bool CanClimbAngle
    {
        get
        {
            return GroundAngle < maxClimbAngle;
        }
    }

    // 디버그용
    protected string Info
    {
        get
        {
            string groundName = GroundCast.collider != null ? GroundCast.transform.name : "";
            return $"PlayerInfo - IsGround: {IsGround}, CanClimbAngle: {CanClimbAngle}, GroundAngle: {GroundAngle}, GroundName: {groundName}, CurrentFriction: {collider.sharedMaterial.friction}";
        }
    }
}