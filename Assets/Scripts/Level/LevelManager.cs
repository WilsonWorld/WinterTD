using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public List<EnemySpawner> Spawners;
    public float PrepTime = 10.0f;

    [SerializeField] bool m_DisableSpawners = false;

    Player m_Player;
    GameObject m_HUD;
    GameObject m_DefeatScreen;
    GameObject m_VictoryScreen;
    GameObject m_PauseMenu;

    float m_PrepTimer = 0.0f;
    int m_WaveNum = 0;
    int m_EnemyNum = 0;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    void Start()
    {
        // Set up variables
        m_PrepTimer = PrepTime;
        m_Player = Camera.main.GetComponent<Player>();
        GameObject userInterfaces = GameObject.Find("User Interfaces");
        m_HUD = userInterfaces.transform.GetChild(0).gameObject;
        m_DefeatScreen = userInterfaces.transform.GetChild(1).gameObject;
        m_VictoryScreen = userInterfaces.transform.GetChild(2).gameObject;
        m_PauseMenu = userInterfaces.transform.GetChild(3).gameObject;

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
            reward += Random.Range(1, 4);
        }

        m_Player.m_MoneyCounter += reward;
        UpdateMoneyCounter();

        if (m_WaveNum < 30)
            PrepareNextWave();
        else
            OpenVictoryScreen();
    }

    // If the spawner is active then update the Wave Counter, reset the prep time/spawn timer to give the player time to prepare for the next wave
    void PrepareNextWave()
    {
        if (Spawners == null || m_DisableSpawners == true)
            return;

        m_WaveNum++;
        UpdateWaveCounter();

        foreach (var spawner in Spawners) {
            spawner.SpawnDelayTime = PrepTime;
            spawner.InitPrefab(m_WaveNum);
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
        if (m_EnemyNum <= 0)
            return;

        m_EnemyNum--;

        if (m_EnemyNum <= 0) {
            m_EnemyNum = 0;
            CompleteWave();
        }

        UpdateEnemyCounter();
    }

    // Clear any remaining towers, reset game varialbles and the UI elements
    public void RestartLevel()
    {
        ClearTowers();

        m_WaveNum = 0;
        m_Player.m_MoneyCounter = 15;
        m_Player.m_LifeCounter = 20;

        UpdateWaveCounter();
        UpdateMoneyCounter();
        UpdateLifeCounter();

        PrepareNextWave();
    }

    /* HUD Functions */

    public void UpdatePrepTimer()
    {
        GameObject prepTimer = m_HUD.transform.GetChild(1).GetChild(0).gameObject;
        if (m_PrepTimer > 0)
            prepTimer.GetComponent<Text>().text = "Next Wave In " + m_PrepTimer.ToString("0");
        else
            prepTimer.GetComponent<Text>().text = "";
    }

    public void UpdateWaveCounter()
    {
        GameObject waveCounter = m_HUD.transform.GetChild(2).GetChild(0).gameObject;
        waveCounter.GetComponent<Text>().text = "Wave " + m_WaveNum.ToString();
    }

    public void UpdateEnemyCounter()
    {
        GameObject enemyCounter = m_HUD.transform.GetChild(3).GetChild(0).gameObject;
        enemyCounter.GetComponent<Text>().text = "Enemies " + m_EnemyNum.ToString();
    }

    public void UpdateMoneyCounter()
    {
        GameObject moneyCounter = m_HUD.transform.GetChild(4).GetChild(0).gameObject;
        moneyCounter.GetComponent<Text>().text = "$ " + m_Player.m_MoneyCounter.ToString();
    }

    public void UpdateLifeCounter()
    {
        GameObject lifeCounter = m_HUD.transform.GetChild(5).GetChild(0).gameObject;
        lifeCounter.GetComponent<Text>().text = "Lives " + m_Player.m_LifeCounter.ToString();
    }

    /* UI Functions */

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

    public void OpenVictoryScreen()
    {
        m_HUD.SetActive(false);
        m_VictoryScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void CloseVictoryScreen()
    {
        m_HUD.SetActive(true);
        m_VictoryScreen.SetActive(false);
        Time.timeScale = 1;
    }

    public void OpenPauseMenu()
    {
        m_PauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    /* Variable Functions */
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
