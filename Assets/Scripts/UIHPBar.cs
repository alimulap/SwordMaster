// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class UIHPBar : MonoBehaviour
{
    public Entity entity;
    public float barWidth = 3.75f;
    public uint barCount = 16;

    RectTransform rect;
    RectTransform remaining;

    // Transform full;
    // Transform remaining;

    void Start()
    {
        this.rect = GetComponent<RectTransform>();
        this.entity.healthChanged.AddListener(this.UpdateRemaining);
        this.remaining = this.rect.Find("Remaining").GetComponent<RectTransform>();
        this.UpdateRemaining();
    }

    void Update() { }

    void UpdateRemaining()
    {
        float fraction = this.entity.Health <= 0 ? 0 : (this.entity.Health / this.entity.MaxHealth);
        int remainingBar = Mathf.CeilToInt((float)this.barCount * fraction);

        var remainingSize = this.remaining.sizeDelta;
        remainingSize.x = this.barWidth * remainingBar;
        this.remaining.sizeDelta = remainingSize;

        // this.remaining.transform.localScale = new(
        //     this.full.transform.localScale.x * fraction,
        //     remainingScale.y,
        //     remainingScale.z
        // );
        // if (this.entity.Health <= 0)
        //     this.gameObject.SetActive(false);
    }
}
