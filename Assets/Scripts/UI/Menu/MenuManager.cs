using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject _mainContent;   
    [SerializeField] private GameObject _settingsPanel; 
    [SerializeField] private GameObject _playPanel;   
    [SerializeField] private GameObject _settingsButton;
    [SerializeField] private GameObject _leaderBoardPanel;
    [SerializeField] private GameObject _leaderBoardButton;

    private void CloseMainPanel()
    {
        _mainContent.SetActive(false); 
        _settingsPanel.SetActive(false); 
        _settingsButton.SetActive(false);
        _leaderBoardButton.SetActive(false);
    }
    public void OpenPlayPanel()
    {
        CloseMainPanel();
        _playPanel.SetActive(true);   
    }

    public void ArcadeButton()
    {
        SceneManager.LoadScene("Arcade"); 
    }
    public void OpenLeaderBoardPanel()
    {
        CloseMainPanel();
        _leaderBoardPanel.SetActive(true);   
    }
    
    public void ClosePlayPanel()
    {
        _playPanel.SetActive(false);   
        _leaderBoardPanel.SetActive(false);
        _leaderBoardButton.SetActive(true);
        _mainContent.SetActive(true);  
        _settingsButton.SetActive(true);
    }
    
    public void ToggleSettings()
    {
        bool isSettingsOpen = _settingsPanel.activeSelf;
        _settingsPanel.SetActive(!isSettingsOpen);
        _mainContent.SetActive(isSettingsOpen);
        
    }
    
    public void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}