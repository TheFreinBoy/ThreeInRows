using UnityEngine;
using DG.Tweening;
using UI;

public class LevelWonDisplay : MonoBehaviour
{
    [SerializeField] private GameObject _board;          
    [SerializeField] private GameObject _winPanel; 
    [SerializeField] private GameObject _score;
    [SerializeField] private GameObject _time;
    [SerializeField] private GameObject _pauseButton;
    
    [SerializeField] private GameStateService _gameState;
    
    public PauseManager pauseManager;

    private void OnEnable()
    {
        if (_gameState != null)
        {
            _gameState.OnLevelWon += HandleLevelWon;
        }
    }

    private void OnDisable()
    {
        if (_gameState != null)
        {
            _gameState.OnLevelWon -= HandleLevelWon;
        }
    }

    private void DisActiveBoard()
    {
        _board.SetActive(false);
        _score.SetActive(false);
        _time.SetActive(false);
        _pauseButton.SetActive(false);
    }
    
    private void HandleLevelWon()
    {
        if (_board != null)
            DisActiveBoard();  

        if (_winPanel != null)
        {
            pauseManager.SetPauseEnabled(false);
            _winPanel.SetActive(true); 
            _winPanel.transform.localScale = Vector3.zero;
            
            _winPanel.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        }
    }
}