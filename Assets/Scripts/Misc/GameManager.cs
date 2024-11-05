using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private PlayerHUD playerHudScript;

    private int playerScore;
    private int playerHighScore = 0;

    public bool isMenuActive = true;

    public AudioSource energizingBackgroundMusic;
    public AudioSource eerieBackgroundMusic;
    public AudioSource crowSounds;

    private Light2D playerLight;

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

        StartCoroutine(UpdateBackgroundMusic());
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

        playerLight = GameObject.FindWithTag("PlayerLight").GetComponent<Light2D>();
        playerLight.pointLightOuterRadius = 24;

        StartCoroutine(ReduceLight());
    }

    private IEnumerator UpdateBackgroundMusic()
    {
        while (energizingBackgroundMusic.volume > 0)
        {
            yield return new WaitForSeconds(0.15f);

            energizingBackgroundMusic.volume -= 0.005f;
            eerieBackgroundMusic.volume += 0.01f;
            crowSounds.volume += 0.007f;
        }
    }

    private IEnumerator ReduceLight()
    {
        while (playerLight.pointLightOuterRadius > 4f)
        {
            yield return new WaitForSeconds(0.5f);

            playerLight.pointLightOuterRadius -= 0.1f;
        }
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

    public void UpdateMenu(bool open)
    {
        if (open)
        {
            isMenuActive = true;
            FindObjectOfType<HeroKnight>().isRunning = false;
        }
        else
        {
            isMenuActive = false;
            FindObjectOfType<HeroKnight>().isRunning = true;
        }
    }
}
