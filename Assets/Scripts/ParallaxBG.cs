// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class ParallaxBG : MonoBehaviour
{
    public Transform target;

    float yOffset;

    void Start()
    {
        this.yOffset = this.transform.position.y;
    }

    void Update()
    {
        var pos = this.transform.position;
        pos.y = this.target.position.y + this.yOffset;
        this.transform.position = pos;
    }
}
