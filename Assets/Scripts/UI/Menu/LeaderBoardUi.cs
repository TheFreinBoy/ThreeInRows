using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class LeaderboardUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _leaderboardText; 
    
    private void OnEnable()
    {
        List<Leaderboard.PlayerScore> topScores = Leaderboard.GetScores();

        _leaderboardText.text = "Leaderboard:\n\n";

        if (topScores.Count == 0)
        {
            _leaderboardText.text += "there are no records yet";
            return;
        }

        for (int i = 0; i < topScores.Count; i++)
        {
            _leaderboardText.text += $"{i + 1}. {topScores[i].playerName}: {topScores[i].score}\n";
        }
    }
}