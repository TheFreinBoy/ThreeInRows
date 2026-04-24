using DG.Tweening;
using UnityEngine;
using TMPro;
using UI.Menu;

public class ScoreService : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;    
    [SerializeField] private TMP_Text _finalScoreText; 
    
    [Header("Сервисы")]
    [SerializeField] private GameStateService _gameState; 

    private int _score;

    void Start()
    {
        _score = 0;
        _scoreText.text = "0";

        if (_gameState != null)
        {
            _gameState.OnGameOver += UpdateFinalScore;
        }
    }

    private void OnDestroy()
    {

        if (_gameState != null)
        {
            _gameState.OnGameOver -= UpdateFinalScore;
        }
    }

    public void AddScore(int score)
    {
        _score += score;
        _scoreText.text = _score.ToString();
        
        if (DOTween.IsTweening(_scoreText.transform))
            DOTween.Kill(_scoreText.transform);
            
        DOTween.Sequence()
            .Append(_scoreText.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.3f))
            .Append(_scoreText.transform.DOScale(Vector3.one, 0.3f));
    }

    private void UpdateFinalScore()
    {
        string currentPlayer = PlayerPrefs.GetString("CurrentPlayerName", "Player");
        Leaderboard.SaveScore(currentPlayer, _score);
        if (_finalScoreText != null)
        {
            _finalScoreText.text = "0";

            float countDuration = 1.5f;

            DOVirtual.Int(0, _score, countDuration, (currentValue) =>
            {
                _finalScoreText.text = currentValue.ToString();
                
            }).SetEase(Ease.OutQuad); 
        }
    }
}