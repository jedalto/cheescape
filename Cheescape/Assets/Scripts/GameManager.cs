using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button menuButton;

    public bool isPaused = false;
    public float timeRemaining = 10;
    public bool timerIsRunning = false;
    public Text timeText;

    void Awake()
    {
        // Ensure pause panel is initially inactive
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
    }

    void Start()
    {
        // Find references if not set in inspector
        if (pausePanel == null)
        {
            pausePanel = GameObject.FindGameObjectWithTag("Pause");
        }

        if (resumeButton == null)
        {
            resumeButton = GameObject.FindGameObjectWithTag("Resume")?.GetComponent<Button>();
        }

        if (menuButton == null)
        {
            menuButton = GameObject.FindGameObjectWithTag("ReturnToMenu")?.GetComponent<Button>();
        }

        if (timeText == null)
        {
            timeText = GameObject.Find("Timer")?.GetComponent<Text>();
        }

        // Ensure game starts unpaused
        Time.timeScale = 1;

        // Start the timer automatically
        timerIsRunning = true;

        // Ensure buttons are linked to their respective methods
        if (resumeButton != null)
        {
            resumeButton.onClick.AddListener(resumeGame);
        }

        if (menuButton != null)
        {
            menuButton.onClick.AddListener(returnToMenu);
        }
    }

    void Update()
    {
        // Check for Escape key to toggle pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                pauseGame();
            }
            else
            {
                resumeGame();
            }
        }

        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }
    }

    // Pause Game
    public void pauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;

        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Pause Panel is not assigned!");
        }
    }

    // Resume Game Function
    public void resumeGame()
    {
        isPaused = false;
        Time.timeScale = 1;

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
    }

    // Return to Menu Function
    public void returnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        if (timeText != null)
        {
            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}
