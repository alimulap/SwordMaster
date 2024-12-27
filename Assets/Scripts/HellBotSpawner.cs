// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class HellBotSpawner : MonoBehaviour
{
    public GameObject hellBot;

    void Start() { }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && GameObject.Find("HellBot") == null)
            Instantiate(this.hellBot, new Vector3(9, 0, 0), Quaternion.identity).name = "HellBot";
    }
}
