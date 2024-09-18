using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerScoreText;
    public TextMeshProUGUI PlayerScoreText
    {
        get => playerScoreText;
        set => playerScoreText = value;
    }

    [SerializeField] private TextMeshProUGUI playerHighScoreText;
    public TextMeshProUGUI PlayerHighScoreText
    {
        get => playerHighScoreText;
        set => playerHighScoreText = value;
    }
}
