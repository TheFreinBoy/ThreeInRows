using System;
using UnityEngine;

public class GameStateService : MonoBehaviour
{
    private bool _isGameOver;
    
    public event Action OnGameOver;
    public bool IsGameOver => _isGameOver;

    public void Initialize()
    {
        _isGameOver = false;
    }

    public void EndGame()
    {
        if (_isGameOver)
            return;

        _isGameOver = true;
        OnGameOver?.Invoke();
    }

    public void ResetGame()
    {
        _isGameOver = false;
    }
}