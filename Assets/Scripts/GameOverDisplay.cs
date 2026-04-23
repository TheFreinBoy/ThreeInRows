using UnityEngine;
using DG.Tweening;

public class GameOverDisplay : MonoBehaviour
{
    [SerializeField] private GameObject _board;          
    [SerializeField] private GameObject _endGamePanel;
    [SerializeField] private GameObject _score;
    [SerializeField] private GameObject _time;
    
    [SerializeField] private GameStateService _gameState; 

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
    }
    
    private void HandleGameOver()
    {
        if (_board != null)
            DisActiveBoard();  

        if (_endGamePanel != null)
            _endGamePanel.SetActive(true); 
        _endGamePanel.transform.localScale = Vector3.zero;
        
        _endGamePanel.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        
    }
}