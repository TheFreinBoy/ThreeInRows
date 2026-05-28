using UnityEngine;
using Fusion;

namespace Multiplayer
{
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