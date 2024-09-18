using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Animations;
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

    [SerializeField] private GameObject playerAttackTextPrefab;

    public void UpdatePlayerAttackInfo(int attackCombo)
    {
        GameObject playerAttackTextObject = Instantiate(playerAttackTextPrefab);
        
        float posX = playerAttackTextObject.transform.localPosition.x;
        float posY = playerAttackTextObject.transform.localPosition.y;

        playerAttackTextObject.transform.SetParent(gameObject.transform);
        playerAttackTextObject.transform.localPosition = new Vector3(posX, posY, 0);

        switch (attackCombo)
        {
            case 1:
                playerAttackTextObject.GetComponent<TextMeshProUGUI>().text = "LIGHT";
                break;
            case 2:
                playerAttackTextObject.GetComponent<TextMeshProUGUI>().text = "MEDIUM";
                break;
            case 3:
                playerAttackTextObject.GetComponent<TextMeshProUGUI>().text = "HEAVY";
                break;
        }

        StartCoroutine(DestroyTextAfterTime(playerAttackTextObject, 0.5f));
    }

    private IEnumerator DestroyTextAfterTime(GameObject textObject, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(textObject);
    }
}
