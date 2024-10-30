using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleRoll : Obstacle
{
    ObstacleRoll()
    {
        fearAmount = 10f;
    }

    protected override void PlayerCollision()
    {
        if (playerScript.Rolling)
        {
            Destroy(gameObject);
            return;
        }

        base.PlayerCollision();
    }
}
