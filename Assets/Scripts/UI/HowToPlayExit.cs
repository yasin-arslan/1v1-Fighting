using UnityEngine;
using UnityEngine.SceneManagement;

public class HowToPlayExit : MonoBehaviour
{
    public void goBack()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
