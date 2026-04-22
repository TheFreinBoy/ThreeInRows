using UnityEngine;
using DG.Tweening;

public class GameOverDisplay : MonoBehaviour
{
    [Header("Ссылки на UI")]
    [SerializeField] private GameObject _board;          
    [SerializeField] private GameObject _endGamePanel; 

    [Header("Сервис состояния")]
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
    
    private void HandleGameOver()
    {
        if (_board != null)
            _board.SetActive(false); 

        if (_endGamePanel != null)
            _endGamePanel.SetActive(true); 
        _endGamePanel.transform.localScale = Vector3.zero;
        
        _endGamePanel.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        
    }
}