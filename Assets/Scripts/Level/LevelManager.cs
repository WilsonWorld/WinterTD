using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public List<EnemySpawner> Spawners;
    public float PrepTime = 10.0f;

    [SerializeField] bool m_DisableSpawners = false;

    Player m_Player;
    GameObject m_HUD;
    GameObject m_DefeatScreen;

    float m_PrepTimer = 0.0f;
    int m_WaveNum = 0;
    int m_EnemyNum = 0;
    bool m_IsGameOver = false;


    void Start()
    {
        // Set up variables
        m_PrepTimer = PrepTime;
        m_Player = Camera.main.GetComponent<Player>();
        GameObject userInterfaces = GameObject.Find("User Interfaces");
        m_HUD = userInterfaces.transform.GetChild(0).gameObject;
        m_DefeatScreen = userInterfaces.transform.GetChild(1).gameObject;

        // Set up the game
        PrepareNextWave();

        // Active and update the HUD
        m_HUD.gameObject.SetActive(true);
        UpdatePrepTimer();
        UpdateMoneyCounter();
        UpdateLifeCounter();

    }

    void Update()
    {
        if (m_PrepTimer > 0.0f) {
            m_PrepTimer -= Time.deltaTime;
            UpdatePrepTimer();
        }
    }

    // When the player survives a wave increase their money, update the UI element for it, and call setup function for next wave.
    void CompleteWave()
    {
        int reward = 2;
        int rewardAmount = m_WaveNum;
        for (int i = 0; i < rewardAmount; i++) {
            reward += Random.Range(1, 5);
        }

        m_Player.m_MoneyCounter += reward;
        UpdateMoneyCounter();
        PrepareNextWave();
    }

    // If the spawner is active then update the Wave Counter, reset the prep time/spawn timer to give the player time to prepare for the next wave
    void PrepareNextWave()
    {
        if (Spawners == null || m_DisableSpawners == true || IsGameOver == true)
            return;

        m_WaveNum++;
        UpdateWaveCounter();

        foreach (var spawner in Spawners) {
            spawner.SpawnDelayTime = PrepTime;
            spawner.StartSpawnTimer();
            m_PrepTimer = PrepTime;
        }
    }

    // Find all the tower objects in the level and remove them
    void ClearTowers()
    {
        GameObject[] remainingTowers = GameObject.FindGameObjectsWithTag("Tower");
        if (remainingTowers != null) {
            foreach (var tower in remainingTowers) {
                Destroy(tower);
            }
        }
    }

    // Increase the Enemy Counter and update the UI element for it.
    public void IncreaseEnemyCounter()
    {
        m_EnemyNum++;
        UpdateEnemyCounter();
    }

    // Decree the Enemy Counter and update the UI element for it. If no enemies remain, end the current wave and move onto the next.
    public void ReduceEnemyCounter()
    {
        m_EnemyNum--;
        UpdateEnemyCounter();

        if (m_EnemyNum <= 0) {
            CompleteWave();
        }
    }

    // Clear any remaining towers, reset game varialbles and the UI elements
    public void RestartLevel()
    {
        ClearTowers();

        m_IsGameOver = false;
        m_WaveNum = 1;
        m_Player.m_MoneyCounter = 10;
        m_Player.m_LifeCounter = 10;

        UpdateWaveCounter();
        UpdateMoneyCounter();
        UpdateLifeCounter();
    }

    /* HUD Functions */

    public void UpdateWaveCounter()
    {
        GameObject waveCounter = m_HUD.transform.GetChild(5).GetChild(0).gameObject;
        waveCounter.GetComponent<Text>().text = "Wave " + m_WaveNum.ToString();
    }

    public void UpdateEnemyCounter()
    {
        GameObject enemyCounter = m_HUD.transform.GetChild(4).GetChild(0).gameObject;
        enemyCounter.GetComponent<Text>().text = "Enemies " + m_EnemyNum.ToString();
    }

    public void UpdatePrepTimer()
    {
        GameObject prepTimer = m_HUD.transform.GetChild(3).GetChild(0).gameObject;
        prepTimer.GetComponent<Text>().text = "Next Wave In " + m_PrepTimer.ToString("0");
    }

    public void UpdateMoneyCounter()
    {
        GameObject moneyCounter = m_HUD.transform.GetChild(2).GetChild(0).gameObject;
        moneyCounter.GetComponent<Text>().text = "$ " + m_Player.m_MoneyCounter.ToString();
    }

    public void UpdateLifeCounter()
    {
        GameObject lifeCounter = m_HUD.transform.GetChild(1).GetChild(0).gameObject;
        lifeCounter.GetComponent<Text>().text = "Lives " + m_Player.m_LifeCounter.ToString();
    }

    public void OpenDefeatScreen()
    {
        m_HUD.SetActive(false);
        m_DefeatScreen.SetActive(true);

        Time.timeScale = 0;
    }

    public void CloseDefeatScreen()
    {
        m_HUD.SetActive(true);
        m_DefeatScreen.SetActive(false);

        Time.timeScale = 1;
    }

    /* Variable Functions */

    public bool IsGameOver
    {
        get { return m_IsGameOver; }
        set { m_IsGameOver = value; }
    }

    public int WaveNum
    {
        get { return m_WaveNum; }
        set { m_WaveNum = value; }
    }

    public Player PlayerRef
    {
        get { return m_Player; }
        private set { m_Player = value; }
    }
}
