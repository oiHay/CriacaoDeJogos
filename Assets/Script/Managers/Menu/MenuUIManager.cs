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
    
    [Header("Panels")] 
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject creditsPanel;

    public enum MainMenuButtons{ Play, Settings, Credits }
    public static MenuUIManager Instance { get; private set; }

    private GameObject _currentPanel;
    
    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            DebugMessage("There are more than one MenuUIManager in this scene");
    }

    public void ButtonClicked(MainMenuButtons buttonClicked)
    {
        DebugMessage("Button Clicked: " + buttonClicked.ToString());

        switch (buttonClicked)
        {
            case MainMenuButtons.Play:
                CustomSceneManager.LoadNextScene();
                break;
            
            case MainMenuButtons.Settings:
                OpenPanel(settingsPanel);
                break;
            
            case MainMenuButtons.Credits:
                OpenPanel(creditsPanel);
                break;
            
            default:
                Debug.Log("Button clicked wasn't implemented in MenuUIManager Method");
                break;
        }
    }

    private void OpenPanel(GameObject panel)
    {
        if (panel == null) return;
        
        if(_currentPanel != null)
            _currentPanel.SetActive(false);

        _currentPanel = panel;
        _currentPanel.SetActive(true);
    }

    private void CloseCurrentPanel()
    {
        if(_currentPanel == null) return;
        
        _currentPanel.SetActive(false);
        _currentPanel = null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseCurrentPanel();
        }
    }
}
