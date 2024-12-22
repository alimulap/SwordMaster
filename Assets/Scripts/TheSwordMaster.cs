using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheSwordMaster : MonoBehaviour
{
    Animator animator;
    bool isDashing = false;

    void Start()
    {
        this.animator = GetComponent<Animator>();
    }

    void Update() { }

    void DashEnds()
    {
        this.isDashing = false;
    }
}
