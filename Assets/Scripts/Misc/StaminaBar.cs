using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaBar : MonoBehaviour
{
    private HeroKnight playerScript;
    private Enemy enemyScript;

    private void Start()
    {
        if (transform.parent.GetComponent<HeroKnight>())
        {
            playerScript = transform.parent.GetComponent<HeroKnight>();
        }
        else if (transform.parent.GetComponent<Enemy>())
        {
            enemyScript = transform.parent.GetComponent<Enemy>();
        }
    }

    private void Update()
    {
        float staminaPercentage = playerScript ? playerScript.GetStaminaPercentage() : enemyScript.GetStaminaPercentage();
        transform.Find("Bar").localScale = new Vector3(staminaPercentage, 1);
    }
}
