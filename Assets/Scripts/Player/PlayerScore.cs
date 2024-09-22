using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    private float playerSpeed;

    private void Start()
    {
        playerSpeed = GetComponent<HeroKnight>().Speed;

        StartCoroutine(UpdatePlayerScore());
    }

    private IEnumerator UpdatePlayerScore()
    {
        while (true)
        {
            if (GetComponent<HeroKnight>().isRunning)
            {
                GameManager.Instance.UpdatePlayerScore(1);
            }

            yield return new WaitForSeconds(0.5f);
        }
    }
}
