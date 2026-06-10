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

    [SerializeField] private PlayerStatsRuntimeSO playerStats;

    private GameObject _currentPanel;

    // Called by the Play button via UIButton or Button.OnClick
    public void Play()
    {
        playerStats.Reset();
        CustomSceneManager.LoadNextScene();
    }

    // Called by Settings/Credits buttons — pass the panel to open directly
    public void OpenPanel(GameObject panel)
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
