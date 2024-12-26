// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HelpPanel : MonoBehaviour
{
    public UnityEvent toggled;

    CanvasGroup cg;
    bool active;

    void Start()
    {
        this.cg = this.GetComponent<CanvasGroup>();

        this.active = false;
        this.cg.alpha = 0;
        this.cg.interactable = false;
        this.cg.blocksRaycasts = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            this.Toggle();
        }
    }

    public void Toggle()
    {
        if (this.active)
        {
            this.cg.alpha = 0;
            this.cg.interactable = false;
            this.cg.blocksRaycasts = false;
        }
        else
        {
            this.cg.alpha = 1;
            this.cg.interactable = true;
            this.cg.blocksRaycasts = true;
        }
        this.active = !this.active;
        this.toggled.Invoke();
    }
}
