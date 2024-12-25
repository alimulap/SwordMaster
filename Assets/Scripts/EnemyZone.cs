// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class EnemyZone : MonoBehaviour
{
    public string zoneName;

    Enemy parent;

    void Start()
    {
        this.parent = GetComponentInParent<Enemy>();
    }

    void Update() { }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            this.parent.OnPlayerEnterZone(
                this.zoneName,
                other.gameObject.GetComponent<TheSwordMaster>()
            );
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            this.parent.OnPlayerExitZone(
                this.zoneName,
                other.gameObject.GetComponent<TheSwordMaster>()
            );
        }
    }
}
