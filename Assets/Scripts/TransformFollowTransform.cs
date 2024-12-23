// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class TransformFollowTransform : MonoBehaviour
{
    public Transform target;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        if (this.target)
            this.transform.position = new(
                this.target.position.x,
                this.target.position.y,
                this.transform.position.z
            );
    }
}
