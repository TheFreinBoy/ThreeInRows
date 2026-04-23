using UnityEngine;
using System.Collections.Generic;

public static class Leaderboard
{
    private const string PREFS_KEY = "LeaderboardData";
    private const int MAX_ENTRIES = 5;
    
    [System.Serializable]
    public class PlayerScore
    {
        public string playerName;
        public int score;

        public PlayerScore(string name, int s)
        {
            playerName = name;
            score = s;
        }
    }

    [System.Serializable]
    private class Data
    {
        public List<PlayerScore> scores = new List<PlayerScore>();
    }
    
    public static void SaveScore(string playerName, int score)
    {
        if (score <= 0 || string.IsNullOrEmpty(playerName)) return;

        Data data = LoadData();
        
        PlayerScore existingPlayer = data.scores.Find(p => p.playerName == playerName);

        if (existingPlayer != null)
        {

            if (score > existingPlayer.score)
            {
                existingPlayer.score = score; 
            }
            else
            {
                return; 
            }
        }
        else
        {
            data.scores.Add(new PlayerScore(playerName, score));
        }
        
        data.scores.Sort((a, b) => b.score.CompareTo(a.score));
        
        if (data.scores.Count > MAX_ENTRIES)
            data.scores.RemoveAt(data.scores.Count - 1);

        PlayerPrefs.SetString(PREFS_KEY, JsonUtility.ToJson(data));
        PlayerPrefs.Save();
    }
    
    public static List<PlayerScore> GetScores()
    {
        return LoadData().scores;
    }

    private static Data LoadData()
    {
        if (PlayerPrefs.HasKey(PREFS_KEY))
        {
            string json = PlayerPrefs.GetString(PREFS_KEY);
            return JsonUtility.FromJson<Data>(json) ?? new Data();
        }
        return new Data();
    }
}