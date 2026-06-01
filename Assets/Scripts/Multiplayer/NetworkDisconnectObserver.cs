using UnityEngine;
using Fusion;

namespace Multiplayer
{
    /// <summary>
    /// Monitors the active network session for opponent disconnections. 
    /// If a player leaves an ongoing match or the connection unexpectedly drops, it automatically triggers a win for the remaining player.
    /// </summary>
    public class NetworkDisconnectObserver : MonoBehaviour
    {
        [SerializeField] private GameStateService _gameState;
        
        private NetworkRunner _runner;
        private bool _isMatchActive;
        
        private void Update()
        {
            if (_gameState == null || _gameState.IsGameOver) return;
            
            if (_runner == null)
            {
                _runner = FindFirstObjectByType<NetworkRunner>();
                if (_runner == null) return; 
            }
            
            if (_runner.IsRunning)
            {
                var players = FindObjectsByType<PlayerState>(FindObjectsInactive.Include, FindObjectsSortMode.None);
                
                if (!_isMatchActive && players.Length >= 2)
                {
                    _isMatchActive = true;
                }

                if (_isMatchActive && players.Length < 2)
                {
                    TriggerDisconnectWin();
                }
            }
            else if (_isMatchActive && !_runner.IsRunning)
            {
                TriggerDisconnectWin();
            }
        }

        private void TriggerDisconnectWin()
        {
            _gameState.WinLevel();
        }
    }
}