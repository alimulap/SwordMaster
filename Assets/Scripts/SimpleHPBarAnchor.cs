// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class SimpleHPBarAnchor : MonoBehaviour
{
    void Start() { }

    void Update()
    {
        this.transform.localScale = this.transform.parent.localScale;
    }
}
