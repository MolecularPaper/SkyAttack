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
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float pushSpeed;
    [SerializeField] protected float jumpSpeed;
    [SerializeField] protected float jumpAccel;
    [SerializeField] protected float maxClimbAngle;

    [Space(10)]
    [SerializeField] protected AudioClip[] jumpSound;
    [SerializeField] protected AudioClip[] landSound;
    [SerializeField] protected AudioClip[] footStepSound;

    [HideInInspector] public KeyInfo key = new KeyInfo();
}

public class PlayerExtension : PlayerBase
{
    protected Vector2 GroundCheckPostion
    {
        get
        {
            return new Vector2(transform.position.x, transform.position.y + collider.size.x - 0.2f);
        }
    }

    protected RaycastHit2D GroundCast
    {
        get
        {
            int layerMask = (-1) - (1 << LayerMask.NameToLayer("Player"));
            return Physics2D.CircleCast(GroundCheckPostion, collider.size.x / 2f, Vector2.down, collider.size.y, layerMask);
        }
    }

    protected Vector2 WallCastPostion
    {
        get
        {
            return transform.position + new Vector3(0.02f * forward, collider.size.y / 2f);
        }
    }


    protected Vector2 WallCastSize
    {
        get
        {
            return new Vector2(collider.size.x, collider.size.y - 0.2f);
        }
    }

    protected Vector2 WallCastDirection
    {
        get
        {
            return Vector3.right * forward;
        }
    }


    protected RaycastHit2D WallCast
    {
        get
        { 
            int layerMask = 1 << LayerMask.NameToLayer("Terrain");
            return Physics2D.CapsuleCast(WallCastPostion, WallCastSize, CapsuleDirection2D.Vertical, 0.0f, WallCastDirection, collider.size.x / 2f, layerMask);
        }
    }

    protected Vector2 TopCehckPostion
    {
        get
        {
            return transform.position + new Vector3(0f, collider.size.y + 0.2f);
        }
    }


    protected Vector2 TopCastSize
    {
        get
        {
            return new Vector2(collider.size.x - 0.02f, 0.1f);
        }
    }


    protected RaycastHit2D TopCast
    {
        get
        {
            int layerMask = 1 << LayerMask.NameToLayer("Terrain");
            return Physics2D.BoxCast(TopCehckPostion, TopCastSize, 0, transform.up, 0.01f, layerMask: layerMask);
        }
    }

    protected bool IsMove
    {
        get
        {
            return key.Horizontal != 0;
        }
    }

    protected bool IsWall
    {
        get
        {
            return WallCast.collider != null;
        }
    }

    protected bool IsGround
    {
        get
        {
            return GroundCast.collider != null;
        }
    }

    protected float GroundAngle
    {
        get
        {
            return Vector2.Angle(GroundCast.normal, Vector2.up);
        }
    }

    protected float GroundAnlgeDir
    {
        get
        {
            return GroundCast.normal.x < 0 ? 1 : -1;
        }
    }

    protected bool CanClimbAngle
    {
        get
        {
            return GroundAngle < maxClimbAngle;
        }
    }

    protected string Info
    {
        get
        {
            string groundName = GroundCast.collider != null ? GroundCast.transform.name : "";
            return $"PlayerInfo - IsGround: {IsGround}, CanClimbAngle: {CanClimbAngle}, GroundAngle: {GroundAngle}, GroundName: {groundName}, CurrentFriction: {collider.sharedMaterial.friction}";
        }
    }
}

public class PlayerTirggers : PlayerExtension
{
    protected bool IsUp;
    protected bool IsTurn;
    protected bool IsJump;
    protected bool IsDamaged;
}