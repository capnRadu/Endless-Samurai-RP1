using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleJump : Obstacle
{
    public override void PlayerCollision()
    {
        base.PlayerCollision();

        RestartLevel();
    }
}
