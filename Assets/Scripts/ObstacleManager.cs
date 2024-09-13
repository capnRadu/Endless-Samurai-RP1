using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] private HeroKnight heroKnight;
    [SerializeField] private List<Obstacle> obstacles;
    private float spawnDistance;

    private void Start()
    {
        spawnDistance = heroKnight.gameObject.transform.position.x + 8;

        StartCoroutine(SpawnObstacles());
    }

    private IEnumerator SpawnObstacles()
    {
        while (true)
        {
            spawnDistance += 8;
            Obstacle obstacle = obstacles[Random.Range(0, obstacles.Count)];
            Instantiate(obstacle, new Vector3(spawnDistance, obstacle.transform.position.y, obstacle.transform.position.z), Quaternion.identity);

            yield return new WaitForSeconds(1);
        }
    }
}
