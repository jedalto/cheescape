using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject pausePanel;
    public Button resumeButton;
    public Button menuButton;
    //public Button quitButton;

    public bool isPaused;

    public float timeRemaining = 10;
    public bool timerIsRunning = false;

    public Text timeText;

    // Start is called before the first frame update
    void Start()
    {
        // Find references again after scene reload
        pausePanel = GameObject.FindGameObjectWithTag("Pause");

        resumeButton = GameObject.FindGameObjectWithTag("Resume").GetComponent<Button>();
        menuButton = GameObject.FindGameObjectWithTag("ReturnToMenu").GetComponent<Button>();
        //quitButton = GameObject.FindGameObjectWithTag("QuitButton").GetComponent<Button>();

        pausePanel.SetActive(false);

        timeText = GameObject.Find("Timer").GetComponent<Text>(); ;

        // Starts the timer automatically
        timerIsRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused) // Pause Game
        {
            isPaused = true;
            Time.timeScale = 0;
            pausePanel.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Escape) && isPaused) // Resume Game
        {
            isPaused = false;
            Time.timeScale = 1;
            pausePanel.SetActive(false);
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

    // Resume Game Function
    public void resumeGame()
    {
        isPaused = false;
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }
    // Return to Menu Function
    public void returnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    //// Quit Game Function
    //public void quitGame()
    //{

    //}

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
