// using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Entity : CharacterController2D
{
    public UnityEvent healthChanged;

    protected HashSet<Effect> effects = new();

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

    public virtual void Apply(Effect effect) { }

    public virtual bool Immobilized
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
                        return true;
                }
            }
            return false;
        }
    }

    protected virtual void UpdateEffect()
    {
        this.effects.RemoveWhere((effect) => Time.time >= effect.startTime + effect.duration);
    }

    public virtual void OnTargetEnterAttack(Collider2D col) { }
}
