using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class Manager : MonoBehaviour
    {

        public void RetryGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void ExitToMenu()
        {


            SceneManager.LoadScene("MainMenu");
        }

    }
}
