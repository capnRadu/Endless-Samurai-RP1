using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBlock : Obstacle
{
    public override void PlayerCollision()
    {
        base.PlayerCollision();
        
        if (playerScript.IsBlocking)
        {
            Destroy(gameObject);
        }
        else
        {
            RestartLevel();
        }
    }
}
