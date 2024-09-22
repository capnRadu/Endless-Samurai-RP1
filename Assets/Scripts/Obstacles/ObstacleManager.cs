using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] private HeroKnight heroKnight;
    [SerializeField] private List<GameObject> obstacles;
    private float spawnDistance;
    private int spawnedObstacles = 0;

    private void Start()
    {
        spawnDistance = heroKnight.gameObject.transform.position.x + 12;

        StartCoroutine(SpawnObstacles());
    }

    private IEnumerator SpawnObstacles()
    {
        while (true)
        {
            if (GameObject.FindWithTag("Enemy") != null)
            {
                yield return null;
            }
            else
            {
                spawnDistance += 12;
                spawnedObstacles++;

                GameObject obstacle;

                if (spawnedObstacles % 4 == 0)
                {
                    obstacle = obstacles[obstacles.Count - 1];
                }
                else
                {
                    obstacle = obstacles[Random.Range(0, obstacles.Count - 1)];
                }

                Instantiate(obstacle, new Vector3(spawnDistance, obstacle.transform.position.y, obstacle.transform.position.z), Quaternion.identity);

                yield return new WaitForSeconds(3);
            }
        }
    }
}
