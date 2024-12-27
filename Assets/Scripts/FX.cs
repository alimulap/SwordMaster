// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class FX : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        this.animator = GetComponent<Animator>();
    }

    public void Trigger(string name, Direction dir)
    {
        var scale = this.transform.localScale;
        switch (dir)
        {
            case Direction.Right:
                scale.x = Mathf.Abs(scale.x);
                this.transform.localScale = scale;
                break;
            case Direction.Left:
                scale.x = -Mathf.Abs(scale.x);
                this.transform.localScale = scale;
                break;
        }

        this.animator.SetTrigger(name);
    }
}
