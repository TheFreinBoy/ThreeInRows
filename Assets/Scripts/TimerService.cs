using System;
using UnityEngine;
using TMPro;

public class TimerService : MonoBehaviour
{
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private float _initialTime = 30f; 
    [SerializeField] private float _timeAddOnMatch = 1f; 
    
    private float _remainingTime;
    private bool _isRunning;
    
    public event Action OnTimeExpired;

    
    public void Initialize(float time)
    {
        _remainingTime = time;
        _isRunning = true;
        UpdateTimerDisplay();
    }

    void Start()
    {
        _remainingTime = _initialTime;
        _isRunning = true;
        UpdateTimerDisplay();
    }

    void Update()
    {
        if (!_isRunning)
            return;

        _remainingTime -= Time.deltaTime;

        if (_remainingTime <= 0)
        {
            _remainingTime = 0;
            _isRunning = false;
            OnTimeExpired?.Invoke();
        }

        UpdateTimerDisplay();
    }

    private void AddTime(float amount)
    {
        _remainingTime += amount;
        UpdateTimerDisplay();
    }

    public void AddTimeOnMatch(int matchSize)
    {
        AddTime(_timeAddOnMatch);
    }

    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(_remainingTime / 60f);
        int seconds = Mathf.FloorToInt(_remainingTime % 60f);
        _timerText.text = $"{minutes:00}:{seconds:00}";
    }

    public void ResetTimer()
    {
        _remainingTime = _initialTime;
        _isRunning = true;
        UpdateTimerDisplay();
    }

    public void StopTimer()
    {
        _isRunning = false;
    }
}

