using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    [Header("Menu Objects")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject creditsMenu;
    
    [Header("Audio Sliders")]
    [SerializeField] private Slider mainSlide;
    [SerializeField] private Slider musicSlide;
    [SerializeField] private Slider sfxSlide;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ShowMainMenu();
        
        mainSlide.value = PlayerPrefs.GetFloat("MasterVol",1f);
        musicSlide.value = PlayerPrefs.GetFloat("MusicVol",1f);
        sfxSlide.value = PlayerPrefs.GetFloat("SFXVol",1f);
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
}
