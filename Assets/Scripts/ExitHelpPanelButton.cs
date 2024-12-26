// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitHelpPanelButton : MonoBehaviour
{
    public HelpPanel helpPanel;
    Button button;

    void Start()
    {
        this.button = GetComponent<Button>();
        this.button.onClick.AddListener(this.ToggleHelpPanel);
    }

    void Update() { }

    void ToggleHelpPanel()
    {
        this.helpPanel.Toggle();
    }
}
