using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;

public class GameManager : MonoBehaviour, IGameManager
{
    [Header("UI Setting")]
    public static GameManager instance;
    [SerializeField] private TextMeshProUGUI timeText;

    #region Panel
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameWinPanel;
    [SerializeField] private GameObject selectDiffPanel;
    #endregion

    public float currentTime;
    public static bool isPaused = false;

    [Header("Atlas Setting")]
    public SpriteAtlas itemAtlas;
    public SpriteAtlas characterAtlas;
    public SpriteAtlas UIAtlas;

    [Header("Difficulty Setting")]
    public DifficultData currentDifficultData;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        var systems = FindObjectsByType<EventSystem>(FindObjectsSortMode.None);
        if (systems.Length > 1)
        {
            for (int i = 1; i < systems.Length; i++)
            {
                Destroy(systems[i].gameObject);
            }
        }
    }

    private void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        if (selectDiffPanel != null)
            selectDiffPanel.SetActive(false);
        if (gameWinPanel != null)
            gameWinPanel.SetActive(false);

        SaveAndLoadDifficult();
    }

    private void SaveAndLoadDifficult()
    {
        string diff = PlayerPrefs.GetString("Difficulty", "Easy");

        currentDifficultData = Resources.Load<DifficultData>($"Difficulties/{diff}");
    }

    void Update()
    {
        if (timeText != null && currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            ShowTime(currentTime);
        }

        //if (currentTime <= 0) GameWin();
    }

    public void ShowTime(float timeToShow)
    {
        if (timeToShow < 0) timeToShow = 0;

        timeText.text = string.Format("{0:00}:{1:00}",
            Mathf.FloorToInt(timeToShow / 60),
            Mathf.FloorToInt(timeToShow % 60));
    }


    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
    }

    public void GameResume()
    {
        Time.timeScale = 1f;
        Input.ResetInputAxes();
        isPaused = false;
    }

    public void GamePause()
    {
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void GameWin()
    {
        GamePause();
        if (gameWinPanel != null)
            gameWinPanel.SetActive(true);
    }

    public void GameQuit()
    {
        Application.Quit();
    }

    public void GameRestart()
    {
        Time.timeScale = 1f;  // Reset timeScale trước khi load scene
        isPaused = false;     // Reset flag
        SceneManager.LoadScene("Game");
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
        if (selectDiffPanel != null)
            selectDiffPanel.SetActive(false);
        if (timeText != null)
            timeText.enabled = true;
        GameResume();
    }

    public void SelectDifficulty()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        if (selectDiffPanel != null)
            selectDiffPanel.SetActive(true);
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);
    }
}
