using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
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
        float healthPercentage = playerScript ? playerScript.GetHealthPercentage() : enemyScript.GetHealthPercentage();
        transform.Find("Bar").localScale = new Vector3(healthPercentage, 1);
    }
}
