using System;
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

    private InputSystem_Actions controls;
    private bool isPaused = false;

    private void Awake()
    {
        controls = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        controls.UI.Enable();
        controls.UI.Cancel.performed += OnCancel;
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
        
        mainSlide.value = PlayerPrefs.GetFloat("MasterVol",1f);
        musicSlide.value = PlayerPrefs.GetFloat("MusicVol",1f);
        sfxSlide.value = PlayerPrefs.GetFloat("SFXVol",1f);
        
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
    }
    
    
    public void ExitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
        
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
        
        // SFX
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Pause", 0);
    }
}
