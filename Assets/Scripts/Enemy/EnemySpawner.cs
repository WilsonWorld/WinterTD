using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [HideInInspector] public float SpawnDelayTime = 3.0f;
    [HideInInspector] public int NumEnemiesToSpawn = 1;

    [SerializeField] List<GameObject> m_Waypoints;
    [SerializeField] LevelManager m_LevelManager;
    [SerializeField] GameObject m_EnemyPrefab;

    // Set up each required enemy's spawn positon, and set their waypoints & health, and create them.
    void SpawnEnemies(int numEnemies)
    {
        for (int x = 0; x < numEnemies; x++) {
            Vector3 spawnPoint = transform.position;;
            spawnPoint.y = transform.position.y + transform.lossyScale.y * 0.5f;

            GameObject enemy = m_EnemyPrefab;
            enemy.GetComponent<Enemy>().Waypoints = m_Waypoints;

            Instantiate(enemy, spawnPoint, transform.rotation);

            m_LevelManager.IncreaseEnemyCounter();
        }
    }

    // Wait a specified time before spawning enemies
    IEnumerator SpawnTimer()
    {
        yield return new WaitForSeconds(SpawnDelayTime);

        SpawnEnemies(NumEnemiesToSpawn);
        m_LevelManager.UpdateEnemyCounter();
    }

    public void StartSpawnTimer()
    {
        StartCoroutine(SpawnTimer());
    }
}
