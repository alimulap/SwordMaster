// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class HellBot : Enemy
{
    public float speed = .2f;
    public Vector2 gravity = new Vector2(0, -0.98f);

    TheSwordMaster player;
    Animator animator;
    bool playerInRadarZone = false;
    bool playerInRangedZone = false;
    bool playerInMeleeZone = false;
    bool isAttacking = false;
    bool isShooting = false;

    public override void Start()
    {
        this.animator = GetComponent<Animator>();
        base.Start();
    }

    public override void Update() { }

    public override void FixedUpdate()
    {
        this.velocity += this.gravity * Time.fixedDeltaTime;
        if (!this.isAttacking && !this.isShooting)
        {
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
                float x_distance = this.player.transform.position.x - this.transform.position.x;
                this.velocity.x = (x_distance > 0 ? this.speed : -this.speed);
            }
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
        col.GetComponent<Entity>().Damage(10);
    }

    public override void Damage(float amount)
    {
        this.Health -= amount;
        // Debug.Log("HellBot health: " + this.Health);
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
