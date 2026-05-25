using System;
using UnityEngine;

public class MenuUIManager : MonoBehaviour
{
    #region Debug

    [SerializeField] private bool debugMode;

    private void DebugMessage(string message)
    {
        if(debugMode)
            Debug.Log(message);
    }

    #endregion
    
    public static MenuUIManager Instance { get; private set; }
    
    public enum MainMenuButtons{ Play, Settings, Credits }
    
    [Header("Panels")] 
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject creditsPanel;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            DebugMessage("There are more than one MenuUIManager in this scene");
    }

    public void ButtonClicked(MainMenuButtons buttonClicked)
    {
        DebugMessage("Button Clicked " + buttonClicked.ToString());
    }
}
