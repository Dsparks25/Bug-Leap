using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton instance of GameManager
    public static GameManager Instance;

    private bool inGame;
    private bool isVictory = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        NewGame();
    }

    private void NewGame()
    {
        SceneManager.LoadScene("Main");
        AudioManager.instance.PlayBackgroundMusic();
        inGame = true;
        isVictory = false;
    }

    // Reset the level with a delay
    public void ResetLevel(float delay)
    {
        Invoke(nameof(ResetLevel), delay);
    }

    public void ResetLevel()
    {
      
        NewGame();
        
    }

    public void Victory()
    {
        isVictory = true;
        SceneManager.LoadScene("Victory");
        ResetLevel(3f);
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Quit the game
            QuitGame();
        }
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
