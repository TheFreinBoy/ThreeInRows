using Fusion;
using TMPro;
using UnityEngine;

namespace Multiplayer
{
    public class NetworkTimerService : NetworkBehaviour
    {
        [SerializeField] private TMP_Text _timerText;
        [SerializeField] private float _matchDuration = 60f;
        [Networked] private float RemainingTime { get; set; }
        [Networked] private bool IsTimerStarted { get; set; }
        
        private bool _isGameFinishedLocal; 

        public override void Spawned()
        {
            if (HasStateAuthority)
            {
                RemainingTime = _matchDuration;
                IsTimerStarted = false; 
            }
            _isGameFinishedLocal = false;
        }
        
        public void StartTimer()
        {
            if (HasStateAuthority)
            {
                IsTimerStarted = true;
            }
        }

        public override void FixedUpdateNetwork()
        {
            if (HasStateAuthority && IsTimerStarted && RemainingTime > 0)
            {
                RemainingTime -= Runner.DeltaTime;
                if (RemainingTime < 0) RemainingTime = 0;
            }
        }

        public override void Render()
        {
            UpdateTimerDisplay(RemainingTime);
            
            if (IsTimerStarted && RemainingTime <= 0 && !_isGameFinishedLocal)
            {
                _isGameFinishedLocal = true;
                DetermineWinner();
            }
        }

        private void UpdateTimerDisplay(float timeToDisplay)
        {
            int minutes = Mathf.FloorToInt(timeToDisplay / 60f);
            int seconds = Mathf.FloorToInt(timeToDisplay % 60f);
            _timerText.text = $"{minutes:00}:{seconds:00}";
        }

        private void DetermineWinner()
        {
            var gameState = FindFirstObjectByType<GameStateService>();
            if (gameState == null) return;

            var players = FindObjectsByType<PlayerState>(FindObjectsSortMode.None);
            //if (players.Length < 2) return;

            PlayerState myPlayer = null;
            PlayerState enemyPlayer = null;
            
            foreach (var p in players)
            {
                if (p.HasStateAuthority) myPlayer = p;
                else enemyPlayer = p;
            }

            if (myPlayer == null || enemyPlayer == null) return;
            
            if (myPlayer.Score > enemyPlayer.Score)
            {
                gameState.WinLevel(); 
            }
            else if (myPlayer.Score < enemyPlayer.Score)
            {
                gameState.EndGame(); 
            }
            else
            {
                gameState.EndGame(); 
            }
        }
    }
}