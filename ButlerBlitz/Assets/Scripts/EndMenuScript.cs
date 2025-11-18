using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenuScript : MonoBehaviour
{
    public void Continue()
    {
        SceneManager.LoadScene("StartMenu");
    }

    void Update()
    {
        if (Input.GetKey("return"))
        {
                    SceneManager.LoadScene("StartMenu");


        }
        if (Input.GetKey("escape"))
        {
                   SceneManager.LoadScene("StartMenu");

        }
    }
}
