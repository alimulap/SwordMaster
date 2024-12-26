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

    public EffectType Type;
}

public class KnockbackEffect : Effect
{
    public KnockbackEffect()
    {
        this.duration = 0.2f;
    }

    public new EffectType Type => EffectType.Knockback;
}

public enum EffectType
{
    Hit,
    Knockback,
}
