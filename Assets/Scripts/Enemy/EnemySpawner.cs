using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [HideInInspector] public float SpawnDelayTime = 3.0f;

    [SerializeField] List<GameObject> m_Waypoints;
    [SerializeField] List<GameObject> m_EnemyPrefabs;
    LevelManager m_LevelManager;
    GameObject m_SpawnPrefab;

    void Start()
    {
        m_LevelManager = LevelManager.Instance;
    }

    // Set up each required enemy's spawn positon, and set their waypoints & health, and create them.
    void SpawnEnemy()
    {
        m_SpawnPrefab.GetComponent<Enemy>().Waypoints = m_Waypoints;
        Vector3 spawnPoint = transform.position;
        spawnPoint.y = transform.position.y + transform.lossyScale.y * 0.5f;
        Instantiate(m_SpawnPrefab, spawnPoint, transform.rotation);

        m_LevelManager.IncreaseEnemyCounter();
    }

    // Wait a specified time before spawning enemies
    IEnumerator SpawnTimer()
    {
        yield return new WaitForSeconds(SpawnDelayTime);

        SpawnEnemy();
        m_LevelManager.UpdateEnemyCounter();
    }

    public void StartSpawnTimer()
    {
        StartCoroutine(SpawnTimer());
    }

    public void InitPrefab(int cWave)
    {
        switch (cWave) {
            case 1:
                m_SpawnPrefab = m_EnemyPrefabs[0];      // Basic
                m_SpawnPrefab.GetComponent<Enemy>().Rank = EnemyRank.RANK_BRONZE;
                break;
            case 2:
                m_SpawnPrefab = m_EnemyPrefabs[1];      // Mobile
                m_SpawnPrefab.GetComponent<Enemy>().Rank = EnemyRank.RANK_BRONZE;
                break;
            case 3:
                m_SpawnPrefab = m_EnemyPrefabs[0];
                m_SpawnPrefab.GetComponent<Enemy>().Rank = EnemyRank.RANK_BRONZE;
                break;
            case 4:
                m_SpawnPrefab = m_EnemyPrefabs[2];      // Heavy
                m_SpawnPrefab.GetComponent<Enemy>().Rank = EnemyRank.RANK_BRONZE;
                break;
            case 5:
                m_SpawnPrefab = m_EnemyPrefabs[0];
                m_SpawnPrefab.GetComponent<Enemy>().Rank = EnemyRank.RANK_BRONZE;
                break;
            case 6:
                m_SpawnPrefab = m_EnemyPrefabs[3];      // Air
                m_SpawnPrefab.GetComponent<Enemy>().Rank = EnemyRank.RANK_BRONZE;
                break;
            case 7:
                m_SpawnPrefab = m_EnemyPrefabs[0];
                m_SpawnPrefab.GetComponent<Enemy>().Rank = EnemyRank.RANK_BRONZE;
                break;
            case 8:
                m_SpawnPrefab = m_EnemyPrefabs[1];      // Mobile
                m_SpawnPrefab.GetComponent<Enemy>().Rank = EnemyRank.RANK_BRONZE;
                break;
            case 9:
                m_SpawnPrefab = m_EnemyPrefabs[0];
                m_SpawnPrefab.GetComponent<Enemy>().Rank = EnemyRank.RANK_BRONZE;
                break;
            case 10:
                m_SpawnPrefab = m_EnemyPrefabs[4];      // BOSS
                m_SpawnPrefab.GetComponent<Enemy>().Rank = EnemyRank.RANK_BRONZE;
                break;
            case 11:
                m_SpawnPrefab = m_EnemyPrefabs[0];
                m_SpawnPrefab.GetComponent<Enemy>().Rank = EnemyRank.RANK_SILVER;
                break;
            case 12:
                m_SpawnPrefab = m_EnemyPrefabs[2];      // Heavy
                m_SpawnPrefab.GetComponent<Enemy>().Rank = EnemyRank.RANK_SILVER;
                break;
            case 13:
                m_SpawnPrefab = m_EnemyPrefabs[0];
                m_SpawnPrefab.GetComponent<Enemy>().Rank = EnemyRank.RANK_SILVER;
                break;
            case 14:
                m_SpawnPrefab = m_EnemyPrefabs[3];      // Air
                m_SpawnPrefab.GetComponent<Enemy>().Rank = EnemyRank.RANK_SILVER;
                break;
            case 15:
                m_SpawnPrefab = m_EnemyPrefabs[0];
                m_SpawnPrefab.GetComponent<Enemy>().Rank = EnemyRank.RANK_SILVER;
                break;
            case 16:
                m_SpawnPrefab = m_EnemyPrefabs[1];      // Mobile
                m_SpawnPrefab.GetComponent<Enemy>().Rank = EnemyRank.RANK_SILVER;
                break;
            case 17:
                m_SpawnPrefab = m_EnemyPrefabs[0];
                m_SpawnPrefab.GetComponent<Enemy>().Rank = EnemyRank.RANK_SILVER;
                break;
            case 18:
                m_SpawnPrefab = m_EnemyPrefabs[2];      // Heavy
                m_SpawnPrefab.GetComponent<Enemy>().Rank = EnemyRank.RANK_SILVER;
                break;
            case 19:
                m_SpawnPrefab = m_EnemyPrefabs[0];
                m_SpawnPrefab.GetComponent<Enemy>().Rank = EnemyRank.RANK_SILVER;
                break;
            case 20:
                m_SpawnPrefab = m_EnemyPrefabs[4];      // BOSS
                m_SpawnPrefab.GetComponent<Enemy>().Rank = EnemyRank.RANK_SILVER;
                break;
            case 21:
                m_SpawnPrefab = m_EnemyPrefabs[0];
                m_SpawnPrefab.GetComponent<Enemy>().Rank = EnemyRank.RANK_GOLD;
                break;
            case 22:
                m_SpawnPrefab = m_EnemyPrefabs[3];      // Air
                m_SpawnPrefab.GetComponent<Enemy>().Rank = EnemyRank.RANK_GOLD;
                break;
            case 23:
                m_SpawnPrefab = m_EnemyPrefabs[0];
                m_SpawnPrefab.GetComponent<Enemy>().Rank = EnemyRank.RANK_GOLD;
                break;
            case 24:
                m_SpawnPrefab = m_EnemyPrefabs[1];      // Mobile
                m_SpawnPrefab.GetComponent<Enemy>().Rank = EnemyRank.RANK_GOLD;
                break;
            case 25:
                m_SpawnPrefab = m_EnemyPrefabs[0];
                m_SpawnPrefab.GetComponent<Enemy>().Rank = EnemyRank.RANK_GOLD;
                break;
            case 26:
                m_SpawnPrefab = m_EnemyPrefabs[2];      // Heavy
                m_SpawnPrefab.GetComponent<Enemy>().Rank = EnemyRank.RANK_GOLD;
                break;
            case 27:
                m_SpawnPrefab = m_EnemyPrefabs[0];
                m_SpawnPrefab.GetComponent<Enemy>().Rank = EnemyRank.RANK_GOLD;
                break;
            case 28:
                m_SpawnPrefab = m_EnemyPrefabs[3];      // Air
                m_SpawnPrefab.GetComponent<Enemy>().Rank = EnemyRank.RANK_GOLD;
                break;
            case 29:
                m_SpawnPrefab = m_EnemyPrefabs[0];
                m_SpawnPrefab.GetComponent<Enemy>().Rank = EnemyRank.RANK_GOLD;
                break;
            case 30:
                m_SpawnPrefab = m_EnemyPrefabs[4];      // BOSS
                m_SpawnPrefab.GetComponent<Enemy>().Rank = EnemyRank.RANK_GOLD;
                break;
        }
    }
}
