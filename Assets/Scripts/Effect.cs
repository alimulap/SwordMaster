// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

public abstract class Effect
{
    public float duration;
    public float startTime;

    public Effect() { }

    public Effect(float duration)
    {
        this.duration = duration;
    }

    public virtual EffectType Type { get; }
}

public class Hit : Effect
{
    new float duration = 0.3f;
    public override EffectType Type => EffectType.Hit;
}

public class KnockbackEffect : Effect
{
    new float duration = 0.3f;
    public override EffectType Type => EffectType.Knockback;
}

public class KnockUp : Effect
{
    public float force = 0.03f;
    new float duration = 0.5f;
    public override EffectType Type => EffectType.KnockUp;
}

public enum EffectType
{
    Hit,
    Knockback,
    KnockUp,
}
