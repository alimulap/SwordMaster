// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class SimpleHPBar : MonoBehaviour
{
    Entity entity;
    Transform full;
    Transform remaining;

    void Start()
    {
        this.entity = GetComponentInParent<Transform>().GetComponentInParent<Entity>();
        this.entity.healthChanged.AddListener(this.UpdateRemaining);
        this.full = this.transform.Find("Full");
        this.remaining = this.transform.Find("Remaining");
        this.UpdateRemaining();
    }

    void Update() { }

    void UpdateRemaining()
    {
        float fraction = this.entity.Health <= 0 ? 0 : (this.entity.Health / this.entity.MaxHealth);
        var remainingScale = this.remaining.transform.localScale;
        this.remaining.transform.localScale = new(
            this.full.transform.localScale.x * fraction,
            remainingScale.y,
            remainingScale.z
        );
        if (this.entity.Health <= 0)
            this.gameObject.SetActive(false);
    }
}
