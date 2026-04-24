using DG.Tweening;
using UnityEngine;
using TMPro;
using UI.Menu;

public class ScoreService : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;    
    
    [Header("Тексты финала (перетащи из панелей!)")]
    [SerializeField] private TMP_Text _winFinalScoreText;   // Текст из панели Победы
    [SerializeField] private TMP_Text _loseFinalScoreText;  // Текст из панели Поражения
    
    [Header("Сервисы")]
    [SerializeField] private GameStateService _gameState; 

    private int _currentScore;
    private int _targetScore;

    public void Initialize(int target)
    {
        _targetScore = target;
        _currentScore = 0;
        UpdateScoreDisplay();
    }

    void Start()
    {
        if (_gameState != null)
        {
            // Подписываемся на ОБА исхода игры
            _gameState.OnGameOver += UpdateLoseScore;
            _gameState.OnLevelWon += UpdateWinScore;
        }
    }

    private void OnDestroy()
    {
        if (_gameState != null)
        {
            // Отписываемся от обоих
            _gameState.OnGameOver -= UpdateLoseScore;
            _gameState.OnLevelWon -= UpdateWinScore;
        }
    }

    public void AddScore(int score)
    {
        _currentScore += score;
        UpdateScoreDisplay();
        
        if (_currentScore >= _targetScore && _targetScore > 0)
        {
            if (_gameState != null)
            {
                _gameState.WinLevel();
            }
        }
        
        if (DOTween.IsTweening(_scoreText.transform))
            DOTween.Kill(_scoreText.transform);
            
        DOTween.Sequence()
            .Append(_scoreText.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.3f))
            .Append(_scoreText.transform.DOScale(Vector3.one, 0.3f));
    }

    private void UpdateScoreDisplay()
    {
        if (_scoreText)
        {
            if (_targetScore > 0)
                _scoreText.text = $"{_currentScore} / {_targetScore}";
            else
                _scoreText.text = _currentScore.ToString();
        }
    }

    // --- МЕТОДЫ ДЛЯ ФИНАЛА ---
    private void UpdateLoseScore() => AnimateFinalScore(_loseFinalScoreText);
    private void UpdateWinScore() => AnimateFinalScore(_winFinalScoreText);

    private void AnimateFinalScore(TMP_Text targetText)
    {
        string currentPlayer = PlayerPrefs.GetString("CurrentPlayerName", "Player");
        Leaderboard.SaveScore(currentPlayer, _currentScore);
        
        if (targetText != null)
        {
            targetText.text = "0";

            // .SetUpdate(true) заставляет DOTween работать даже если игра на паузе!
            DOVirtual.Int(0, _currentScore, 1.5f, (currentValue) =>
            {
                targetText.text = currentValue.ToString();
                
            }).SetEase(Ease.OutQuad).SetUpdate(true); 
        }
    }
}