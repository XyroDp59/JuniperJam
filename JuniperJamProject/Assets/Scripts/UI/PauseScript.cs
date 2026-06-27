using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Slider mainSlide;
    [SerializeField] private Slider musicSlide;
    [SerializeField] private Slider sfxSlide;
    [SerializeField] private PlayerScript player;

    [Header("Transition Animation")]
    [SerializeField] private Animator transitionAnimator;

    private InputSystem_Actions controls;
    private bool isPaused = false;

    private void Awake()
    private void OnEnable()
    {
        if (controls == null) { controls = player.GetInputActions(); }
        controls.UI.Enable();
        controls.UI.Cancel.performed += OnCancel;
        controls.UI.Click.performed += ctx => Debug.Log("Clicked!");
    }

    private void OnDisable()
    {
        controls.UI.Disable();
        controls.UI.Cancel.performed -= OnCancel;
    }

    private void OnCancel(InputAction.CallbackContext ctx) => TogglePause();

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
        pausePanel.SetActive(isPaused);

        player.TogglePlayerInput(!isPaused);
        Debug.Log($"isPaused: {isPaused} | UI enabled: {controls.UI.enabled} | Player enabled: {controls.Player.enabled}");


        mainSlide.value = PlayerPrefs.GetFloat("MasterVol",1f);
        musicSlide.value = PlayerPrefs.GetFloat("MusicVol",1f);
        sfxSlide.value = PlayerPrefs.GetFloat("SFXVol",1f);
        Debug.Assert(FMODUnity.RuntimeManager.StudioSystem.isValid(), "[PauseMenu] FMOD was not valid");
        // SFX
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Pause", isPaused ? 1 : 0);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pausePanel.SetActive(false);
        
        mainSlide.value = PlayerPrefs.GetFloat("MasterVol",1f);
        musicSlide.value = PlayerPrefs.GetFloat("MusicVol",1f);
        sfxSlide.value = PlayerPrefs.GetFloat("SFXVol",1f);

        StartCoroutine(FadeInTransition());
    }
    
    
    public void ExitToMainMenu()
    {
        Time.timeScale = 1f;
        StartCoroutine(FadeOutTransition());
        Debug.Assert(FMODUnity.RuntimeManager.StudioSystem.isValid(), "[PauseMenu] FMOD was not valid");

        // SFX
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Pause", 0);
    }

    public void Resume()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        mainSlide.value = PlayerPrefs.GetFloat("MasterVol",1f);
        musicSlide.value = PlayerPrefs.GetFloat("MusicVol",1f);
        sfxSlide.value = PlayerPrefs.GetFloat("SFXVol",1f);
        Time.timeScale = 1;
        Debug.Assert(FMODUnity.RuntimeManager.StudioSystem.isValid(), "[PauseMenu] FMOD was not valid");

        // SFX
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Pause", 0);
    }

    
    IEnumerator FadeOutTransition()
    {
        transitionAnimator.SetBool("Transition", false);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator FadeInTransition()
    {
        transitionAnimator.SetBool("Transition", true);
        yield return new WaitForSeconds(1);
    }
}
