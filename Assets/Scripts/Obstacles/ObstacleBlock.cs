using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBlock : Obstacle
{
    ObstacleBlock()
    {
        fearAmount = 15f;
    }

    protected override void PlayerCollision()
    {      
        if (playerScript.IsBlocking)
        {
            Destroy(gameObject);
            return;
        }

        base.PlayerCollision();
    }
}
