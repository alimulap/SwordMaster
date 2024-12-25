// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    Entity parent;

    public string targetTag;

    void Start()
    {
        this.parent = this.GetComponentInParent<Entity>();
    }

    void Update() { }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(this.targetTag))
        {
            this.parent.OnTargetEnterAttack(other);
        }
    }
}
