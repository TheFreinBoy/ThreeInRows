using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Menu
{
    public class LevelButton : MonoBehaviour
    {
        [SerializeField] private LevelData _levelToLoad; 
        [SerializeField] private string _gameSceneName = "Arcade"; 
        
        public void LoadLevel()
        {
            GameContext.SelectedLevel = _levelToLoad;
            
            SceneManager.LoadScene(_gameSceneName);
        }
    }
}