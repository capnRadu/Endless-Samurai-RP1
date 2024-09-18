using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Obstacle : MonoBehaviour
{
    protected HeroKnight playerScript;
    protected PlayerHUD playerHUDScript;

    protected void Start()
    {
        playerScript = FindObjectOfType<HeroKnight>();
        playerHUDScript = FindObjectOfType<PlayerHUD>();
    }

    protected void Update()
    {
        if (transform.position.x < playerScript.gameObject.transform.position.x - 2)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == playerScript.gameObject)
        {
            PlayerCollision();
        }
    }

    public virtual void PlayerCollision()
    {
    }

    protected void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}