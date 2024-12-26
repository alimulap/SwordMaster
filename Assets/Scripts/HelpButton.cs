// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpButton : MonoBehaviour
{
    public HelpPanel helpPanel;
    public bool active;
    Button button;
    CanvasGroup cg;

    void Start()
    {
        this.helpPanel.toggled.AddListener(this.Toggle);

        this.button = GetComponent<Button>();
        this.button.onClick.AddListener(this.ToggleHelpPanel);
        this.cg = this.GetComponent<CanvasGroup>();

        this.active = !this.active;
        this.Toggle();
    }

    void Update() { }

    void ToggleHelpPanel()
    {
        this.helpPanel.Toggle();
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
    }
}
