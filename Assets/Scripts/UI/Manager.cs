using UnityEngine;
using UnityEngine.SceneManagement;

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
