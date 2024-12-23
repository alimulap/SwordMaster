using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CharacterController2D : MonoBehaviour
{
    BoxCollider2D col;
    RayCastOrigins rayOrigins = new RayCastOrigins();
    LayerMask collisionMask;
    float rayOffset = 0.001f;

    protected Vector2 velocity = new Vector2(0, 0);
    protected bool isOnGround = false;

    public Vector2 gravity = new Vector2(0, -0.98f);
    public float jumpForce = 30;
    public float moveAcceleration = 10;
    public uint castCount = 3;

    public virtual void Start()
    {
        this.col = GetComponent<BoxCollider2D>();

        //this.gravity = Physics2D.gravity;
        this.collisionMask = LayerMask.GetMask("Ground");
    }

    public virtual void Update() { }

    public virtual void FixedUpdate() { }

    protected void Move()
    {
        this.UpdateRayCastOrigins();
        this.CheckVerticalCollisions();
        this.CheckHorizontalCollisions();
        this.transform.Translate(this.velocity);
    }

    protected void CheckVerticalCollisions()
    {
        if (this.velocity.y != 0)
        {
            int dir = this.velocity.y > 0 ? 1 : -1;
            bool rayHit = false;
            for (uint i = 0; i < this.castCount; i++)
            {
                Vector2 rayOrigin =
                    dir == -1 ? this.rayOrigins.bottomLeft : this.rayOrigins.topLeft;
                rayOrigin +=
                    Vector2.right
                    * (
                        this.rayOffset
                        + (
                            (this.col.bounds.extents.x * 2 - this.rayOffset * 2)
                            / (this.castCount - 1)
                            * i
                        )
                    );
                RaycastHit2D hit = Physics2D.Raycast(
                    rayOrigin,
                    Vector2.up * dir,
                    Mathf.Abs(this.velocity.y),
                    this.collisionMask
                );
                Debug.DrawRay(rayOrigin, Vector3.up * this.velocity.y, Color.red);

                if (hit)
                {
                    this.velocity.y = hit.distance * dir;
                    rayHit = true;
                }
            }
            if (rayHit && dir == -1)
                this.isOnGround = true;
            else
                this.isOnGround = false;
        }
    }

    protected void CheckHorizontalCollisions()
    {
        if (this.velocity.x != 0)
        {
            int dir = this.velocity.x > 0 ? 1 : -1;
            for (uint i = 0; i < this.castCount; i++)
            {
                Vector2 rayOrigin =
                    dir == 1 ? this.rayOrigins.bottomRight : this.rayOrigins.bottomLeft;
                rayOrigin +=
                    Vector2.up
                    * (
                        this.rayOffset
                        + (
                            (this.col.bounds.extents.y * 2 - this.rayOffset * 2)
                            / (this.castCount - 1)
                            * i
                        )
                    );
                RaycastHit2D hit = Physics2D.Raycast(
                    rayOrigin,
                    Vector2.right * dir,
                    Mathf.Abs(this.velocity.x),
                    this.collisionMask
                );
                Debug.DrawRay(rayOrigin, Vector3.right * this.velocity.x, Color.red);
                if (hit)
                    this.velocity.x = hit.distance * dir;
            }
        }
    }

    protected void UpdateRayCastOrigins()
    {
        this.rayOrigins.topRight = this.col.bounds.max;
        this.rayOrigins.topLeft = new Vector2(this.col.bounds.min.x, this.col.bounds.max.y);
        this.rayOrigins.bottomRight = new Vector2(this.col.bounds.max.x, this.col.bounds.min.y);
        this.rayOrigins.bottomLeft = this.col.bounds.min;
    }
}
