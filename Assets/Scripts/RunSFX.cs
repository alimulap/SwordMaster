// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class RunSFX : MonoBehaviour
{
    public Entity target;

    AudioSource source;
    bool active = false;

    void Start()
    {
        this.source = GetComponent<AudioSource>();
        this.source.time = 0.05f;
    }

    void Update()
    {
        if (Mathf.Abs(this.target.Velocity.x) > 0 && this.target.IsOnGround)
        {
            if (!this.active)
            {
                this.source.Play();
                this.active = true;
            }
        }
        else if (this.active)
        {
            this.source.Stop();
            this.active = false;
        }
    }
}
