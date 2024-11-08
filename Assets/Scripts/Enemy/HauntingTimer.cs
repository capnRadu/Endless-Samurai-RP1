using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HauntingTimer : MonoBehaviour
{
    private TextMeshProUGUI timerText;

    public bool isActive = false;
    private float hauntingTime = 10f;
    private float currentTime = 0f;

    private void Start()
    {
        timerText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (isActive)
        {
            currentTime += Time.deltaTime;

            if (currentTime >= hauntingTime)
            {
                isActive = false;
                currentTime = 0f;
                timerText.text = "Cursed!";
                FindObjectOfType<Enemy>().isPlayerCursed = true;
            }
            else
            {
                timerText.text = (hauntingTime - currentTime).ToString("F2");
            }
        }
    }

    public void StartTimer()
    {
        isActive = true;
        timerText.enabled = true;
    }

    public void ResetTimer()
    {
        isActive = false;
        currentTime = 0f;
        timerText.enabled = false;
    }
}
