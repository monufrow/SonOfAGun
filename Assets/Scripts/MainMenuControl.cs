using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuScrpit : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }
    
}
