using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private PlayerHUD playerHudScript;

    private int playerScore;
    private int playerHighScore = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        playerHudScript = FindObjectOfType<PlayerHUD>();

        playerScore = 0;
        playerHudScript.PlayerScoreText.text = $"{playerScore}";

        playerHudScript.PlayerHighScoreText.text = $"HI {playerHighScore}";
    }

    public void UpdatePlayerScore(int score)
    {
        playerScore += score;
        playerHudScript.PlayerScoreText.text = $"{playerScore}";

        if (playerScore > playerHighScore)
        {
            playerHighScore = playerScore;
            playerHudScript.PlayerHighScoreText.text = $"HI {playerHighScore}";
        }
    }
}
