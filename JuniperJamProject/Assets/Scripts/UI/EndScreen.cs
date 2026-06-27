using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    [SerializeField] private GameObject mainUi;
    [SerializeField] private Image panel;
    [SerializeField] private TextMeshProUGUI endText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject buttons;
    [SerializeField] private AttributSet playerAttributeSet;
    
    [Header("Transition Animation")]
    [SerializeField] private Animator transitionAnimator;

    private void Awake()
    {
        playerAttributeSet.onDeath.AddListener(() => StartCoroutine(EndGame()));
    }

    private IEnumerator EndGame()
    {
        mainUi.SetActive(false);
        Time.timeScale = 0.5f;

        var panelColor = panel.color;
        panelColor.a = 0;
        panel.color = panelColor;
        panel.gameObject.SetActive(true);
        float currentShade = 0;
        while (currentShade < 0.125f)
        {
            print (panel.color + "; " + currentShade);
            currentShade += 0.0025f;
            var color = panel.color;
            color.a = currentShade;
            panel.color = color;
            yield return new WaitForSeconds(0.02f);
        }
        Time.timeScale = 0;
        endText.gameObject.SetActive(true);
        //yield return new WaitForSeconds(1f);
        scoreText.text = "For your effort, you achieved to recieve " + ScoreManager.Instance.GetScore() + " dollars.";
        scoreText.gameObject.SetActive(true);
        //yield return new WaitForSeconds(1f);
        buttons.SetActive(true);
        
        // SFX
        SoundtrackController.Instance.endInstance.start();
    }
    
    public void ExitToMainMenu()
    {
        Time.timeScale = 1f;
        StartCoroutine(ExitFadeOutTransition());
        
        // SFX
        //FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Pause", 0);
    }
    
    public void Retry()
    {
        Time.timeScale = 1f;
        StartCoroutine(RetryFadeOutTransition());
        
        // SFX
        //FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Pause", 0);
    }
    
    
    IEnumerator RetryFadeOutTransition()
    {
        transitionAnimator.SetBool("Transition", false);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("GameScene");
    }

    IEnumerator ExitFadeOutTransition()
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
