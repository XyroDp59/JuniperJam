using System.Collections;
using FMODUnity;
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

    [Header("Transition Animation")]
    [SerializeField] private Animator transitionAnimator;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainSlide.value = PlayerPrefs.GetFloat("MasterVol",1f);
        musicSlide.value = PlayerPrefs.GetFloat("MusicVol",1f);
        sfxSlide.value = PlayerPrefs.GetFloat("SFXVol",1f);
        
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        CloseMenu();
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        creditsMenu.SetActive(false);
        StartCoroutine(FadeInTransition());
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
        StartCoroutine(QuitFadeOutTransition());
    }

    public void StartGame()
    {
        StartCoroutine(StartFadeOutTransition());
    }

    IEnumerator QuitFadeOutTransition()
    {
        transitionAnimator.SetBool("Transition", false);
        yield return new WaitForSeconds(1);
        Application.Quit();
    }
    
    IEnumerator StartFadeOutTransition()
    {
        transitionAnimator.SetBool("Transition", false);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(1);
    }

    IEnumerator FadeInTransition()
    {
        transitionAnimator.SetBool("Transition", true);
        yield return new WaitForSeconds(1);
    }
}
