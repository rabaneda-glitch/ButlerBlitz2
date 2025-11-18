using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    private GameObject currentState;

    public enum MenuStates
    {
        Main,
        Levels,
        Options,
        Credits,
    };

    public GameObject mainMenu;
    public GameObject levelMenu;
    public GameObject optionsMenu;
    public GameObject creditsMenu;

    void Start()
    {
        currentState = mainMenu;
        mainMenu.SetActive(true);
        levelMenu.SetActive(false);
        optionsMenu.SetActive(false);
        creditsMenu.SetActive(false);
    }

    public void back()
    {
        Debug.Log("back to main menu");
        switchMenu(MenuStates.Main);
    }

    public void levels()
    {
        Debug.Log("levels selected");
        switchMenu(MenuStates.Levels);
    }
    public void options()
    {
        Debug.Log("options selected");
        switchMenu(MenuStates.Options);
    }
    public void credits()
    {
        Debug.Log("credits selected");
        switchMenu(MenuStates.Credits);
    }

    public void levelOne()
    {
        Debug.Log("level 1 selected");
        SceneManager.LoadScene("MovementButler");
    }



    public void switchMenu(MenuStates menu)
    {
        GameObject newState;

        switch (menu)
        {
            case MenuStates.Main:
                newState = mainMenu;
                break;
            case MenuStates.Levels:
                newState = levelMenu;
                break;
            case MenuStates.Options:
                newState = optionsMenu;
                break;
            case MenuStates.Credits:
                newState = creditsMenu;
                break;
            default:
                newState = mainMenu;
                break;
        }

        // Desactivar el menú anterior antes de cambiar
        if (currentState != null)
            currentState.SetActive(false);

        currentState = newState;
        currentState.SetActive(true);
    }

    void Update()
    {

        if (Input.GetKey("return"))
        {
            if (mainMenu.activeSelf)
            {
                switchMenu(MenuStates.Levels);
            }
            
        }
        if (Input.GetKey("escape"))
        {
            if (mainMenu.activeSelf)
            {
                Application.Quit();
            }
            else
            {
                switchMenu(MenuStates.Main);
            }
        }
    }
}
