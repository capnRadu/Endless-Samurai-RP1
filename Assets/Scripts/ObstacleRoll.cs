using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleRoll : Obstacle
{
    public override void PlayerCollision()
    {
        base.PlayerCollision();

        if (playerScript.Rolling)
        {
            Destroy(gameObject);
        }
        else
        {
            RestartLevel();
        }
    }
}
