// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Entity : CharacterController2D
{
    public UnityEvent healthChanged;

    private float health = 100;
    private float maxHealth = 100;

    public float MaxHealth
    {
        get { return this.maxHealth; }
    }

    public float Health
    {
        get { return this.health; }
        protected set
        {
            this.health = value;
            this.healthChanged.Invoke();
        }
    }

    public virtual void Damage(float amount)
    {
        this.Health -= amount;
    }

    public virtual void Heal(float amount)
    {
        this.health += amount;
    }

    public virtual void OnTargetEnterAttack(Collider2D col) { }
}
