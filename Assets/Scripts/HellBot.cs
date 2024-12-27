// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class HellBot : Enemy
{
    public float speed = .2f;
    public float gravityMultiplier = 0.025f;
    public float meleeCooldown = 1f;
    public float rangedCooldown = 1f;

    TheSwordMaster player;
    Animator animator;
    FX fx;

    Vector2 gravity;
    float lastMeleeTime = -Mathf.Infinity;
    float lastRangedTime = -Mathf.Infinity;

    bool playerInRadarZone = false;
    bool playerInRangedZone = false;
    bool playerInMeleeZone = false;
    bool isAttacking = false;
    bool isShooting = false;
    bool hpzero = false;

    public override void Start()
    {
        this.animator = GetComponent<Animator>();
        this.gravity = Physics2D.gravity;
        this.fx = this.transform.Find("Anchor/FX").GetComponent<FX>();
        base.Start();
    }

    public override void Update()
    {
        if (this.Health <= 0 && !this.hpzero)
        {
            this.hpzero = true;
            this.animator.SetBool("hpzero", true);
            this.animator.SetTrigger("hpzero_trigger");
        }
        this.UpdateEffect();
        // Debug.Log(this.effects.Count);
    }

    public override void FixedUpdate()
    {
        this.velocity += this.gravity * this.gravityMultiplier * Time.fixedDeltaTime;

        if (this.player != null && !this.isAttacking && !this.isShooting)
        {
            float x_distance = this.player.transform.position.x - this.transform.position.x;
            if (
                this.playerInMeleeZone
                && Time.time >= this.lastMeleeTime + this.meleeCooldown
                && this.CanAttack
            )
            {
                this.isAttacking = true;
                this.velocity.x = 0;
                this.animator.SetTrigger("attack");
            }
            else if (
                this.playerInRangedZone
                && Time.time >= this.lastRangedTime + this.rangedCooldown
                && this.CanAttack
            )
            {
                this.isShooting = true;
                this.velocity.x = 0;
                this.animator.SetTrigger("shot");
            }
            else if (this.playerInRadarZone)
            {
                if (!this.Immobilized)
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

    bool CanAttack
    {
        get
        {
            foreach (var effect in this.effects)
            {
                switch (effect.Type)
                {
                    case EffectType.Hit:
                    case EffectType.Knockback:
                    case EffectType.KnockUp:
                        return false;
                }
            }
            return true;
        }
    }

    public override void Apply(Effect effect)
    {
        effect.StartTime = Time.time;
        this.effects.Add(effect);

        switch (effect.Type)
        {
            case EffectType.Hit:
                this.isAttacking = false;
                this.isShooting = false;
                this.animator.SetTrigger("hit");
                this.fx.Trigger("hit3electric", (effect as Hit).fromDirection);
                break;
            case EffectType.KnockUp:
                this.isAttacking = false;
                this.isShooting = false;
                this.velocity.y = (effect as KnockUp).force;
                this.animator.SetTrigger("hit");
                break;
        }
    }

    void FinishAttacking()
    {
        this.isAttacking = false;
        this.lastMeleeTime = Time.time;
    }

    void FinishShooting()
    {
        this.isShooting = false;
        this.lastRangedTime = Time.time;
    }

    void Destroy()
    {
        Destroy(this.gameObject);
    }
}
