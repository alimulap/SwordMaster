// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

public abstract class Effect
{
    public virtual float Duration { get; }
    public virtual float StartTime { get; set; }

    public Effect() { }

    public Effect(float duration)
    {
        this.Duration = duration;
    }

    public virtual EffectType Type { get; }
}

public class Hit : Effect
{
    public Direction fromDirection;
    public override float Duration
    {
        get { return 1f; }
    }
    public override EffectType Type => EffectType.Hit;

    public Hit() { }

    public Hit(Direction dir)
    {
        this.fromDirection = dir;
    }
}

public class KnockbackEffect : Effect
{
    public override float Duration
    {
        get { return 1f; }
    }
    public override EffectType Type => EffectType.Knockback;
}

public class KnockUp : Effect
{
    public float force = 0.03f;
    public override float Duration
    {
        get { return 1f; }
    }
    public override EffectType Type => EffectType.KnockUp;
}

public enum EffectType
{
    Hit,
    Knockback,
    KnockUp,
}
