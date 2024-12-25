// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class HellBot : Enemy
{
    public float speed = .2f;
    public Vector2 gravity = new Vector2(0, -0.98f);

    TheSwordMaster player;
    bool playerInRadarZone = false;
    bool playerInRangedZone = false;
    bool playerInMeleeZone = false;

    public override void Start()
    {
        base.Start();
    }

    public override void Update() { }

    public override void FixedUpdate()
    {
        this.velocity += this.gravity * Time.fixedDeltaTime;
        if (this.playerInMeleeZone)
        {
            this.velocity.x = 0;
        }
        else if (this.playerInRangedZone)
        {
            this.velocity.x = 0;
        }
        else if (this.playerInRadarZone)
        {
            float x_distance = this.player.transform.position.x - this.transform.position.x;
            this.velocity.x = (x_distance > 0 ? this.speed : -this.speed);
        }
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

    public override void Damage(float amount)
    {
        this.Health -= amount;
        Debug.Log("HellBot health: " + this.Health);
    }

    public override void Heal(float amount) { }
}
