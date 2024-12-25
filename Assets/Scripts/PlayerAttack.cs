// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    TheSwordMaster player;

    void Start()
    {
        this.player = GetComponentInParent<TheSwordMaster>();
    }

    void Update() { }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            this.player.OnEnemyEnterAttack(other.GetComponent<Enemy>());
        }
    }
}
