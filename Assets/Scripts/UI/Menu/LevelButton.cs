using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Menu
{
    public class LevelButton : MonoBehaviour
    {
        [SerializeField] private LevelData _levelToLoad; // Перетащи сюда файл Level_1 или Level_2
        [SerializeField] private string _gameSceneName = "Arcade"; // Имя твоей игровой сцены
        
        public void LoadLevel()
        {
            GameContext.SelectedLevel = _levelToLoad;
            
            SceneManager.LoadScene(_gameSceneName);
        }
    }
}