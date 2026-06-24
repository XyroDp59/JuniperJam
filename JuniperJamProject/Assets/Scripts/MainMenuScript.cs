using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject creditsMenu;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        CloseMenu();
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        creditsMenu.SetActive(false);
    }

    public void CloseMenu()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);
        creditsMenu.SetActive(false);
    }

    public void ShowOptionsMenu()
    {
        CloseMenu();
        optionsMenu.SetActive(true);
        mainMenu.SetActive(false);
        creditsMenu.SetActive(false);
    }

    public void ShowCreditsMenu()
    {
        CloseMenu();
        creditsMenu.SetActive(true);
        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    
    // SFX 
    public void HoverButtonSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/hover");
    }
    public void BackButtonSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/back");
    }
    
    public void ConfirmButtonSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/confirm");
    }
    
}
