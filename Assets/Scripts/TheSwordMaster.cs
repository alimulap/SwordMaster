// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TheSwordMaster : Entity
{
    public float gravityMultiplier = 0.025f;
    public float moveSpeed = 0.02f;
    public float jumpForce = 30;
    public float dashSpeed = 0.050f;
    public float rollSpeed = 0.035f;
    public float dashCooldown = 0.5f;
    public float rollCooldown = 0.3f;

    Animator animator;

    Vector2 gravity;
    float lashDashTime = -Mathf.Infinity;
    float lastRollTime = -Mathf.Infinity;

    bool shouldJump = false;

    bool readyNextAtt = true;
    bool comboTriggered = false;
    bool shouldDash = false;
    bool isDashing = false;
    bool shouldRoll = false;
    bool isRolling = false;
    bool hpzero = false;

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
            if (this.isDashing) { }
            else if (this.isRolling)
            {
                this.currAtt = Attack.RollAttack;
                this.animator.SetTrigger("roll_attack");
            }
            else if (this.currAtt.Equals(Attack.None))
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
            Input.GetKeyDown(KeyCode.K)
            && this.currAtt.Equals(Attack.None)
            && !this.isRolling
            && Time.time >= this.lashDashTime + this.dashCooldown + 0.1f
        )
        {
            this.shouldDash = true;
        }

        if (
            Input.GetKeyDown(KeyCode.N)
            && this.currAtt.Equals(Attack.None)
            && !this.isDashing
            && this.isOnGround
            && Time.time >= this.lastRollTime + this.rollCooldown + 0.1f
        )
        {
            this.shouldRoll = true;
        }

        if (
            this.isOnGround
            && this.currAtt == Attack.None
            && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
        )
        {
            this.shouldJump = true;
        }

        bool notDashingNorRolling = !this.isDashing && !this.isRolling;

        if (this.currAtt.Equals(Attack.None))
        {
            bool rightDown = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
            bool leftDown = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);

            switch (rightDown, leftDown, notDashingNorRolling)
            {
                case (true, false, true):
                    this.moveDir = MoveDir.Right;
                    break;
                case (false, true, true):
                    this.moveDir = MoveDir.Left;
                    break;
                default:
                    this.moveDir = MoveDir.None;
                    break;
            }

            this.FacingHandler(rightDown, leftDown);
        }
        else
        {
            this.moveDir = MoveDir.None;
        }

        if (this.Health <= 0 && !this.hpzero)
        {
            this.hpzero = true;
            this.animator.SetTrigger("hpzero");
        }
    }

    public override void FixedUpdate()
    {
        this.Gravitate();
        this.AttackHandler();
        this.JumpHandler();
        this.MovementHandler();
        this.Move();
        this.AnimatorController();
    }

    void AttackHandler() { }

    void JumpHandler()
    {
        if (this.shouldJump)
        {
            this.velocity = Vector2.up * this.jumpForce;
            this.shouldJump = false;
        }
    }

    void MovementHandler()
    {
        if (!this.isDashing && !this.isRolling)
        {
            switch (this.moveDir)
            {
                case MoveDir.Right:
                    if (this.velocity.x < 0)
                        this.velocity = new Vector2(0, this.velocity.y);
                    this.velocity.x = this.moveSpeed;
                    break;
                case MoveDir.Left:
                    if (this.velocity.x > 0)
                        this.velocity = new Vector2(0, this.velocity.y);
                    this.velocity.x = -this.moveSpeed;
                    break;
                case MoveDir.None:
                    this.velocity.x = 0;
                    break;
            }
        }

        if (this.shouldDash)
        {
            int dir = this.facing.Equals(FaceDir.Right) ? 1 : -1;
            this.velocity.x = this.dashSpeed * dir;
            this.isDashing = true;
            this.shouldDash = false;
            this.animator.SetBool("is_dashing", true);
            this.animator.SetTrigger("dash");
        }

        if (this.shouldRoll)
        {
            int dir = this.facing.Equals(FaceDir.Right) ? 1 : -1;
            this.velocity.x = this.rollSpeed * dir;
            this.isRolling = true;
            this.shouldRoll = false;
            this.animator.SetBool("is_rolling", true);
            this.animator.SetTrigger("roll");
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

        bool rightDown = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
        bool leftDown = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);

        this.FacingHandler(rightDown, leftDown);
    }

    void FacingHandler(bool rightDown, bool leftDown)
    {
        switch (rightDown, leftDown)
        {
            case (true, false):
                this.facing = FaceDir.Right;
                break;
            case (false, true):
                this.facing = FaceDir.Left;
                break;
            default:
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

    public override void OnTargetEnterAttack(Collider2D col)
    {
        this.OnEnemyEnterAttack(col.GetComponent<Enemy>());
    }

    public void OnEnemyEnterAttack(Enemy enemy)
    {
        if (this.nextAtt.Equals(Attack.Slam))
            enemy.Apply(new KnockUp());
        else
            enemy.Apply(new Hit());
        enemy.Damage(10);
    }

    void DashEnds()
    {
        this.isDashing = false;
        this.velocity.x = 0;
        this.lashDashTime = Time.time;
        this.animator.SetBool("is_dashing", false);
    }

    // 1 for attack, 0 for not attack
    void RollEnds(int attack)
    {
        bool attackFinished = attack.Equals(1);
        bool isRollAttacking = this.currAtt.Equals(Attack.RollAttack);
        if (!(isRollAttacking ^ attackFinished))
        // if ((isRollAttacking && attackFinished) || (!isRollAttacking && !attackFinished))
        {
            this.isRolling = false;
            this.velocity.x = 0;
            this.currAtt = Attack.None;
            this.lastRollTime = Time.time;
            this.animator.SetBool("is_rolling", false);
        }
    }

    void StopHorizontalMove()
    {
        this.velocity.x = 0;
    }

    public override void Damage(float amount)
    {
        if (!this.isRolling)
            this.Health -= amount;
    }

    void Destroy()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
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
