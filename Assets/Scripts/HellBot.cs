// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class HellBot : Enemy
{
    public float speed = .2f;
    public float gravityMultiplier = 0.025f;

    TheSwordMaster player;
    Animator animator;

    Vector2 gravity;

    bool playerInRadarZone = false;
    bool playerInRangedZone = false;
    bool playerInMeleeZone = false;
    bool isAttacking = false;
    bool isShooting = false;

    public override void Start()
    {
        this.animator = GetComponent<Animator>();
        this.gravity = Physics2D.gravity;
        base.Start();
    }

    public override void Update() { }

    public override void FixedUpdate()
    {
        this.velocity += this.gravity * this.gravityMultiplier * Time.fixedDeltaTime;

        if (this.player != null && !this.isAttacking && !this.isShooting)
        {
            float x_distance = this.player.transform.position.x - this.transform.position.x;
            if (this.playerInMeleeZone)
            {
                this.isAttacking = true;
                this.velocity.x = 0;
                this.animator.SetTrigger("attack");
            }
            else if (this.playerInRangedZone)
            {
                this.isShooting = true;
                this.velocity.x = 0;
                this.animator.SetTrigger("shot");
            }
            else if (this.playerInRadarZone)
            {
                this.velocity.x = (x_distance > 0 ? this.speed : -this.speed);
            }

            if (x_distance > 0)
                this.transform.localScale = new(1, 1, 1);
            else
                this.transform.localScale = new(-1, 1, 1);
        }

        this.animator.SetFloat("x_abs_velocity", Mathf.Abs(this.velocity.x));
        this.Move();
    }

    public override void OnPlayerEnterZone(string zone, TheSwordMaster player)
    {
        switch (zone)
        {
            case "radar":
                this.player = player;
                this.playerInRadarZone = true;
                break;
            case "ranged":
                if (!this.player)
                    this.player = player;
                this.playerInRangedZone = true;
                break;
            case "melee":
                if (!this.player)
                    this.player = player;
                this.playerInMeleeZone = true;
                break;
        }
    }

    public override void OnPlayerExitZone(string zone, TheSwordMaster player)
    {
        switch (zone)
        {
            case "radar":
                this.player = null;
                this.playerInRadarZone = false;
                break;
            case "ranged":
                if (!this.playerInRadarZone)
                    this.player = null;
                this.playerInRangedZone = false;
                break;
            case "melee":
                if (!this.playerInRangedZone && !this.playerInRadarZone)
                    this.player = null;
                this.playerInMeleeZone = false;
                break;
        }
    }

    public override void OnTargetEnterAttack(Collider2D col)
    {
        col.GetComponent<Entity>().Damage(2);
    }

    void FinishAttacking()
    {
        this.isAttacking = false;
    }

    void FinishShooting()
    {
        this.isShooting = false;
    }
}
