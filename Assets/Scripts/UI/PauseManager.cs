using UnityEngine;
using DG.Tweening;

namespace UI
{
    
    public class PauseManager : MonoBehaviour
    {
        [SerializeField] private GameObject _pausePanel; 
        [SerializeField] private GameObject _score;
        [SerializeField] private GameObject _timer;
        [SerializeField] private GameObject _pauseButton;
        [SerializeField] private GameObject _board;

        private bool _isPaused;
        private bool _canPause = true;
        
        private void Start()
        {
            if (_pausePanel != null)
            {
                _pausePanel.SetActive(false);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && _canPause)
            {
                TogglePause();
            }
        }
        private void OnDestroy()
        {
            Time.timeScale = 1f;
        }
        public void SetPauseEnabled(bool isEnabled)
        {
            _canPause = isEnabled;
            
            if (!isEnabled && _isPaused)
            {
                _pausePanel.SetActive(false);
                _isPaused = false;
                Time.timeScale = 1f; 
            }
        }
        
        public void TogglePause()
        {
            if (!_canPause)
                return;
            
            _isPaused = !_isPaused;

            if (_isPaused)
            {
                PauseGame();
                EnablePanels(false);
            }
            else
            {
                ResumeGame();
                EnablePanels(true);
            }
        }

        private void EnablePanels(bool main)
        {
            _score.SetActive(main);
            _timer.SetActive(main);
            _pauseButton.SetActive(main);
            _board.SetActive(main);
        }

        private void PauseGame()
        {
            Time.timeScale = 0f; 
            
            if (_pausePanel != null)
            {
                _pausePanel.SetActive(true);
                
                _pausePanel.transform.localScale = Vector3.zero;
                
                _pausePanel.transform.DOScale(Vector3.one, 0.3f)
                    .SetEase(Ease.OutBack)
                    .SetUpdate(true); 
            }
        }

        private void ResumeGame()
        {
            _isPaused = false;

            if (_pausePanel != null)
            {
                _pausePanel.transform.DOScale(Vector3.zero, 0.2f)
                    .SetEase(Ease.InBack)
                    .SetUpdate(true)
                    .OnComplete(() => 
                    {
                        _pausePanel.SetActive(false);
                        
                        Time.timeScale = 1f; 
                    });
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
    }
}