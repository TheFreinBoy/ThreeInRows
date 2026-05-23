using Fusion;
using TMPro;
using UnityEngine;

namespace Multiplayer
{
    public class PlayerState : NetworkBehaviour
    {
        [Networked] public int Score { get; set; }

        private TMP_Text _scoreTextUI;
        private ChangeDetector _changeDetector;
        private bool _isUIFound;

        public override void Spawned()
        {
            _changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);
        }

        public override void Render()
        {
            if (!_isUIFound)
            {
                FindUIElements();
            }

            if (_changeDetector != null)
            {
                foreach (var change in _changeDetector.DetectChanges(this))
                {
                    if (change == nameof(Score))
                    {
                        UpdateScoreUI();
                    }
                }
            }
        }

        private void FindUIElements()
        {
            TMP_Text[] allTexts = FindObjectsByType<TMP_Text>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            if (allTexts.Length == 0) return;

            GameObject targetTextObj = null;

            if (HasStateAuthority)
            {
                foreach (var t in allTexts)
                {
                    if (t.name == "MyScoreText") targetTextObj = t.gameObject;
                }
            }
            else
            {
                foreach (var t in allTexts)
                {
                    if (t.name == "EnemyScoreText") targetTextObj = t.gameObject;
                }
            }

            if (targetTextObj != null)
            {
                _scoreTextUI = targetTextObj.GetComponent<TMP_Text>();
                _isUIFound = true;
                UpdateScoreUI();
            }
        }

        private void UpdateScoreUI()
        {
            if (_scoreTextUI != null)
            {
                _scoreTextUI.text = Score.ToString();
            }
        }

        public void AddPoints(int points)
        {
            if (HasStateAuthority)
            {
                Score += points;
            }
        }
    }
}