using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Declarations
    public static GameManager instance;
    private bool isPaused;
    private UIManager uiManager;
    [SerializeField] private Animator pauseAnimator;
    #endregion

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
        uiManager = UIManager.instance;
        isPaused = false;
    }
    private void Update()
    {
        PressPause();
    }
    #endregion

    #region Pause
    public void PressPause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Pause()
    {
        Time.timeScale = 0f;
        isPaused = true;
        uiManager.PauseGame();
    }
    public void Resume()
    {
        pauseAnimator.SetTrigger("Exit");
        uiManager.Resume();
        Time.timeScale = 1f;
        isPaused = false;
    }
    #endregion

    #region Restart
    public void Restart()
    {
        uiManager.Restart();
        Time.timeScale = 1f;
        isPaused = false;
    }
    #endregion
}
