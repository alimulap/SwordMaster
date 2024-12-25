// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class TheSwordMaster : Entity
{
    public float moveAcceleration = 10;
    public float jumpForce = 30;
    public float gravityMultiplier = 0.025f;

    Animator animator;

    Vector2 gravity;

    bool shouldJump = false;

    bool readyNextAtt = true;
    bool comboTriggered = false;
    bool isDashing = false;
    bool isRolling = false;

    MoveDir moveDir = MoveDir.None;
    FaceDir facing = FaceDir.Right;
    Attack currAtt = Attack.None;
    Attack nextAtt = Attack.None;

    public override void Start()
    {
        this.animator = GetComponent<Animator>();
        this.gravity = Physics2D.gravity;
        base.Start();
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (this.currAtt.Equals(Attack.None))
            {
                this.currAtt = Attack.Slash1;
                this.nextAtt = Attack.Slash1;
                this.animator.SetTrigger("slash1");
                this.readyNextAtt = false;
            }
            else if (this.readyNextAtt)
            {
                this.comboTriggered = true;
                this.readyNextAtt = false;
                this.animator.SetTrigger("combo");
            }
        }

        if (
            this.isOnGround
            && this.currAtt == Attack.None
            && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
        )
        {
            this.shouldJump = true;
        }

        bool rightDown = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
        bool leftDown = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);

        switch (rightDown, leftDown, this.currAtt == Attack.None)
        {
            case (true, false, true):
                this.moveDir = MoveDir.Right;
                this.facing = FaceDir.Right;
                break;
            case (false, true, true):
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
                this.transform.localScale = new(1, 1, 1);
                break;
            case FaceDir.Left:
                this.transform.localScale = new(-1, 1, 1);
                break;
        }
    }

    public override void FixedUpdate()
    {
        this.Gravitate();
        this.AttackHandler();
        this.JumpHandler();
        this.Accelerate();
        this.Move();
        this.AnimatorController();
    }

    void AttackHandler() { }

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
        this.velocity += this.gravity * this.gravityMultiplier * Time.fixedDeltaTime;
    }

    void ReadyNextAttack()
    {
        this.readyNextAtt = true;
    }

    void AttackFinished()
    {
        if (this.comboTriggered)
        {
            this.comboTriggered = false;
            switch (this.nextAtt)
            {
                case Attack.Slash1:
                    this.currAtt = Attack.Slash1;
                    this.nextAtt = Attack.Slash2;
                    break;
                case Attack.Slash2:
                    this.currAtt = Attack.Slash2;
                    this.nextAtt = Attack.SpinAttack;
                    break;
                case Attack.SpinAttack:
                    this.currAtt = Attack.SpinAttack;
                    this.nextAtt = Attack.Slam;
                    break;
                case Attack.Slam:
                    this.currAtt = Attack.Slam;
                    this.nextAtt = Attack.SpinAttack;
                    break;
                case Attack.RollAttack:
                    this.currAtt = Attack.RollAttack;
                    this.nextAtt = Attack.None;
                    break;
                case Attack.None:
                    this.currAtt = Attack.None;
                    break;
            }
        }
        else
        {
            this.currAtt = Attack.None;
            this.nextAtt = Attack.None;
            this.animator.SetTrigger("exit_combo");
        }
    }

    public override void OnTargetEnterAttack(Collider2D col)
    {
        this.OnEnemyEnterAttack(col.GetComponent<Enemy>());
    }

    public void OnEnemyEnterAttack(Enemy enemy)
    {
        enemy.Damage(10);
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

enum Attack
{
    Slash1,
    Slash2,
    SpinAttack,
    Slam,
    RollAttack,
    None,
}
