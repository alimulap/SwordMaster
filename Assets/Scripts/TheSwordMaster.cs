using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheSwordMaster : CharacterController2D
{
    SpriteRenderer sprite;
    Animator animator;

    bool shouldJump = false;
    bool isJumping = false;
    bool isDashing = false;
    bool isRolling = false;

    MoveDir moveDir = MoveDir.None;
    FaceDir facing = FaceDir.Right;

    public override void Start()
    {
        this.sprite = GetComponent<SpriteRenderer>();
        // this.sprite.transform.localScale = new(-1, 1, 1);
        this.animator = GetComponent<Animator>();
        base.Start();
    }

    public override void Update()
    {
        if (this.isOnGround && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)))
        {
            this.shouldJump = true;
        }

        bool rightDown = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
        bool leftDown = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);

        switch (rightDown, leftDown)
        {
            case (true, false):
                this.moveDir = MoveDir.Right;
                this.facing = FaceDir.Right;
                break;
            case (false, true):
                this.moveDir = MoveDir.Left;
                this.facing = FaceDir.Left;
                break;
            default:
                this.moveDir = MoveDir.None;
                break;
        }

        switch (this.facing)
        {
            case FaceDir.Right:
                // this.sprite.flipX = false;
                break;
            case FaceDir.Left:
                // this.sprite.flipX = true;
                break;
        }
    }

    public override void FixedUpdate()
    {
        this.Gravitate();
        this.JumpHandler();
        this.Accelerate();
        this.Move();
        this.AnimatorController();
        Debug.Log("IsOnGround: " + this.isOnGround);
    }

    void JumpHandler()
    {
        if (this.shouldJump)
        {
            this.velocity += Vector2.up * this.jumpForce * Time.fixedDeltaTime;
            this.shouldJump = false;
        }
    }

    void Accelerate()
    {
        switch (this.moveDir)
        {
            case MoveDir.Right:
                if (this.velocity.x < 0)
                    this.velocity = new Vector2(0, this.velocity.y);
                this.velocity.x += this.moveAcceleration * Time.fixedDeltaTime;
                break;
            case MoveDir.Left:
                if (this.velocity.x > 0)
                    this.velocity = new Vector2(0, this.velocity.y);
                this.velocity.x -= this.moveAcceleration * Time.fixedDeltaTime;
                break;
            case MoveDir.None:
                this.velocity.x = 0;
                break;
        }
    }

    void AnimatorController()
    {
        this.animator.SetFloat("x_velocity", this.velocity.x);
        this.animator.SetFloat("y_velocity", this.velocity.y);
        this.animator.SetFloat("x_abs_velocity", Mathf.Abs(this.velocity.x));
        this.animator.SetBool("is_on_ground", this.isOnGround);
    }

    void Gravitate()
    {
        this.velocity += this.gravity * Time.fixedDeltaTime;
    }

    void DashEnds()
    {
        this.isDashing = false;
    }

    void RollEnds()
    {
        this.isRolling = false;
    }
}

enum MoveDir
{
    Right,
    Left,
    None,
}

enum FaceDir
{
    Right,
    Left,
}
