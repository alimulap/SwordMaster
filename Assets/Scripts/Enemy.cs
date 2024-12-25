// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

public abstract class Enemy : Entity
{
    public virtual void OnPlayerEnterZone(string zone, TheSwordMaster player) { }

    public virtual void OnPlayerExitZone(string zone, TheSwordMaster player) { }
}
