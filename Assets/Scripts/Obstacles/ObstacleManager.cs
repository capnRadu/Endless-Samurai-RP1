using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] private HeroKnight heroKnight;
    [SerializeField] private List<GameObject> obstacles;
    private float spawnDistance;
    private int spawnedObstacles = 0;
    private int enemySpawnRate;

    private void Start()
    {
        spawnDistance = heroKnight.gameObject.transform.position.x + 12;
        enemySpawnRate = Random.Range(4, 7);
        Debug.Log("Enemy spawn rate: " + enemySpawnRate);

        StartCoroutine(SpawnObstacles());
    }

    private IEnumerator SpawnObstacles()
    {
        while (true)
        {
            if (GameObject.FindWithTag("Enemy") != null || GameManager.Instance.isMenuActive)
            {
                yield return null;
            }
            else
            {
                spawnDistance += 12;
                spawnedObstacles++;

                GameObject obstacle;

                if (spawnedObstacles % enemySpawnRate == 0)
                {
                    obstacle = obstacles[obstacles.Count - 1];
                    spawnedObstacles = 0;
                    enemySpawnRate = Random.Range(4, 7);
                    Debug.Log("Enemy spawn rate: " + enemySpawnRate);
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
