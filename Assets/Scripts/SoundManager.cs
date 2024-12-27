// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public void Play(string name)
    {
        AudioSource source = this.transform.Find("Sounds/" + name).GetComponent<AudioSource>();
        if (name.Equals("Dash"))
            source.time = 0.2f;
        if (name.Equals("Jump"))
            source.time = 0.2f;
        source.Play();
    }
}
