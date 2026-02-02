using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    #region Declarations
    public static UIManager instance;
    [Header("UI Panels")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject creditsMenu;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;
    public enum UIScreens { Pause, MainMenu, Options, Credits, Win, Lose }
    private Dictionary<UIScreens, GameObject> uiOrganize;
    #endregion
   

    [SerializeField] AudioClip mainMenuBackground;
    [SerializeField] AudioClip uiButtonClick;
    #region MonoBehaviour
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        
        UIDictionary();
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }
        if (mainMenu != null)
        {
            mainMenu.SetActive(true);
            optionsMenu.SetActive(false);
            creditsMenu.SetActive(false);
        }

        AudioManager.instance.PlaySound(mainMenuBackground);
    }
    #endregion

    #region Screens
    public void UIDictionary()
    {
        uiOrganize = new Dictionary<UIScreens, GameObject>();
        uiOrganize.Add(UIScreens.MainMenu, mainMenu);
        uiOrganize.Add(UIScreens.Options, optionsMenu);
        uiOrganize.Add(UIScreens.Pause, pauseMenu);
        uiOrganize.Add(UIScreens.Credits, creditsMenu);
        uiOrganize.Add(UIScreens.Win, winScreen);
        uiOrganize.Add(UIScreens.Lose, loseScreen);
    }
    public void ShowScreen(UIScreens screens)
    {
        foreach (var screen in uiOrganize.Values)
        {
            if (screen != null) screen.SetActive(false);
        }

        if (uiOrganize.ContainsKey(screens) && uiOrganize[screens] != null)
        {
            uiOrganize[screens].SetActive(true);
        }
    }
    #endregion

    #region MainMenu
    public void StartGame()
    {
        NextScene();
        AudioManager.instance.PlaySound(uiButtonClick);
    }
    public void MainMenu()
    {
        ShowScreen(UIScreens.MainMenu);
        AudioManager.instance.PlaySound(uiButtonClick);
    }
    public void Options()
    {
        ShowScreen(UIScreens.Options);
        //audioSC.Play();
       AudioManager.instance.PlaySound(uiButtonClick);
    }
    public void Credits()
    {
        ShowScreen(UIScreens.Credits);
        AudioManager.instance.PlaySound(uiButtonClick);
    }
    public void Quit()
    {
        Application.Quit();
        AudioManager.instance.PlaySound(uiButtonClick);
    }
    #endregion

    #region EndGameMenus
    public void Win()
    {
        ShowScreen(UIScreens.Win);
    }
    public void Lose()
    {
        ShowScreen(UIScreens.Lose);
    }
    #endregion

    #region Scenes
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void MainMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
    #endregion

    #region PauseMenu
    public void PauseGame()
    {
        ShowScreen(UIScreens.Pause);
        AudioManager.instance.PlaySound(uiButtonClick);
    }
    public void Resume()
    {
        StartCoroutine(Exit());
        AudioManager.instance.PlaySound(uiButtonClick);
    }
    private IEnumerator Exit()
    {
        yield return new WaitForSeconds(2f);
        pauseMenu.SetActive(false);
    }
    #endregion
}
