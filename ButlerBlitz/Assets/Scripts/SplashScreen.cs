using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    public string nextScene = "StartMenu";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("StartMenu");
        }
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(nextScene);
    }
}
