// using System.Collections;
// using System.Collections.Generic;
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

    public abstract void Damage(float amount);

    public abstract void Heal(float amount);
}
