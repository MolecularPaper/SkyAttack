using UnityEngine;



public class PlayerBase : DynamicObjectExtension
{
    [Space(10)]
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float jumpSpeed;
    [SerializeField] protected float jumpAccel;
    [SerializeField] protected float maxClimbAngle;

    protected KeyInfo key = new KeyInfo();
}

public class PlayerExtension : PlayerBase
{
    protected Vector2 GroundCheckPostion
    {
        get
        {
            return new Vector2(transform.position.x, transform.position.y + collider.size.x);
        }
    }

    protected RaycastHit2D GroundCast
    {
        get
        {
            int layerMask = (-1) - (1 << LayerMask.NameToLayer("Player"));
            return Physics2D.CircleCast(GroundCheckPostion, collider.size.x / 2f - 0.05f, -Vector2.up, collider.size.x / 2f + 0.1f, layerMask);
        }
    }

    protected Vector2 WallCehckPostion
    {
        get
        {
            return transform.position + new Vector3((collider.size.x / 2f + 0.1f) * forward, collider.size.y / 2f);
        }
    }


    protected Vector2 WallCastSize
    {
        get
        {
            return new Vector2(0.1f, collider.size.y - 0.02f);
        }
    }

    protected Vector2 WallCastDirection
    {
        get
        {
            return Vector3.right * forward * 0.1f;
        }
    }


    protected RaycastHit2D WallCast
    {
        get
        {
            int layerMask = (1 << LayerMask.NameToLayer("Player")) + (1 << LayerMask.NameToLayer("Terrain"));
            return Physics2D.BoxCast(WallCehckPostion, WallCastSize, 0, WallCastDirection, 0.01f, layerMask: layerMask);
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
}