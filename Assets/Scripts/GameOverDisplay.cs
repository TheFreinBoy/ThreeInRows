using UnityEngine;
using DG.Tweening;
using UI;

public class GameOverDisplay : MonoBehaviour
{
    [SerializeField] private GameObject _board;          
    [SerializeField] private GameObject _endGamePanel;
    [SerializeField] private GameObject _score;
    [SerializeField] private GameObject _time;
    [SerializeField] private GameObject _pauseButton;
    
    [SerializeField] private GameStateService _gameState; 
    
    public PauseManager pauseManager;

    private void OnEnable()
    {
        if (_gameState != null)
        {
            _gameState.OnGameOver += HandleGameOver;
        }
    }

    private void OnDisable()
    {
        if (_gameState != null)
        {
            _gameState.OnGameOver -= HandleGameOver;
        }
    }

    private void DisActiveBoard()
    {
        _board.SetActive(false);
        _score.SetActive(false);
        _time.SetActive(false);
        _pauseButton.SetActive(false);
    }
    
    private void HandleGameOver()
    {
        if (_board != null)
            DisActiveBoard();


        if (_endGamePanel != null)
        {
            pauseManager.SetPauseEnabled(false);
            _endGamePanel.SetActive(true); 
            SoundManager.Instance.PlayLoseSound();
        }
        _endGamePanel.transform.localScale = Vector3.zero;
        
        _endGamePanel.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        
    }
}