using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button menuButton;
    [SerializeField] private GameObject winPanel; // Add a reference to the win panel
    [SerializeField] private GameObject losePanel;
    //[SerializeField] private Text youWin;

    public bool isPaused = false;
    public bool isGameWon = false; // New flag to track game win state

    public float timeRemaining = 600;
    public bool timerIsRunning = false;
    public Text timeText;

    // Reference to the player movement script
    [SerializeField] private PlayerMovement playerMovement;

    void Awake()
    {
        // Ensure pause panel is initially inactive
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        // Ensure win panel is initially inactive
        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }
        if (losePanel != null)
        {
            losePanel.SetActive(false);
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

        //if (youWin == null)
        //{
        //    youWin = GameObject.Find("YouWin")?.GetComponent<Text>();
        //}

        // Find player movement script if not assigned
        if (playerMovement == null)
        {
            playerMovement = FindObjectOfType<PlayerMovement>();
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
        // Check for Escape key to toggle pause (only if game is not won)
        if (Input.GetKeyDown(KeyCode.Escape) && !isGameWon)
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
                isGameWon = true;
                Time.timeScale = 0;
                // Disable player movement
                if (playerMovement != null)
                {
                    playerMovement.enabled = false;
                }

                // Show win panel if assigned
                if (losePanel != null)
                {
                    losePanel.SetActive(true);
                }

                //if (youWin != null)
                //{
                //    youWin.text = "You Lose!";
                //}
            }
        }
    }

    // Pause Game
    public void pauseGame()
    {
        if (isGameWon) return; // Can't pause if game is won

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

        // Disable player movement
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
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

        // Re-enable player movement
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }
    }

    // Win Game Function
    public void WinGame()
    {
        if (isGameWon) return; // Prevent multiple win calls

        isGameWon = true;
        timerIsRunning = false;
        Time.timeScale = 0;

        // Disable player movement
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        // Show win panel if assigned
        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }

        Debug.Log("You Win!");
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
