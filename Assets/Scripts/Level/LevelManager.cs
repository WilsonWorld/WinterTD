using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public GridManager m_GridManager;
    [SerializeField] AudioSource m_sfxSource;
    [SerializeField] AudioSource m_bgmSource;

    [Header("Spawner Options")]
    public List<EnemySpawner> Spawners;
    public float PrepTime = 10.0f;
    [SerializeField] bool m_DisableSpawners = false;

    Player m_Player;
    GameObject m_SelectedUnit;
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

        // Set up the first wave
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

        if (m_SelectedUnit == null) {
            CloseEnemyDisplay();
            return;
        }

        Enemy selectedEnemy = m_SelectedUnit.GetComponent<Enemy>();
        if (selectedEnemy) {
            string healthText = selectedEnemy.CurrentHealth.ToString() + " / " + selectedEnemy.StartingHealth.ToString();
            UpdateEnemyDisplay(selectedEnemy.ProfileSprite, healthText);

            if (selectedEnemy.CurrentHealth <= 0) {
                m_SelectedUnit = null;
                CloseEnemyDisplay();
            }
        }
    }

    // When the player survives a wave increase their money, update the UI element for it, and call setup function for next wave.
    void CompleteWave()
    {
        PlayWaveCompletionSFX();

        int reward = 5 + m_WaveNum;
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
                Destroy(tower.gameObject);
            }
        }
    }

    // Find all the enemy objects in the level and remove them
    void ClearEnemies()
    {
        GameObject[] remainingEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (remainingEnemies != null) {
            foreach (var enemy in remainingEnemies) {
                Destroy(enemy.gameObject);
            }
        }
    }

    // Play completion sfx and lower the bgm to prevent masking the sfx
    void PlayWaveCompletionSFX()
    {
        m_sfxSource.Play();
        m_bgmSource.volume = 0.1f;

        StartCoroutine(sfxSourceDelay());
    }

    IEnumerator sfxSourceDelay()
    {
        float clipLength = m_sfxSource.clip.length - 1.0f;
        yield return new WaitForSeconds(clipLength);

        m_bgmSource.volume = 0.4f;
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
        ClearEnemies();
        m_GridManager.ClearGrid();
        m_GridManager.GenerateGrid();

        m_WaveNum = 0;
        m_Player.m_MoneyCounter = 20;
        m_Player.m_LifeCounter = 20;

        UpdateWaveCounter();
        UpdateMoneyCounter();
        UpdateLifeCounter();

        PrepareNextWave();
    }

    /* HUD Functions */

    public void UpdateWaveCounter()
    {
        GameObject waveCounter = m_HUD.transform.GetChild(1).GetChild(0).gameObject;
        waveCounter.GetComponent<Text>().text = "Wave " + m_WaveNum.ToString();
    }

    public void UpdateEnemyCounter()
    {
        GameObject enemyCounter = m_HUD.transform.GetChild(2).GetChild(0).gameObject;
        enemyCounter.GetComponent<Text>().text = "Enemies " + m_EnemyNum.ToString();
    }

    public void UpdatePrepTimer()
    {
        GameObject prepTimer = m_HUD.transform.GetChild(3).GetChild(0).gameObject;
        if (m_PrepTimer > 0)
            prepTimer.GetComponent<Text>().text = "Next Wave In " + m_PrepTimer.ToString("0");
        else
            prepTimer.GetComponent<Text>().text = "";
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

    public void OpenEnemyDisplay()
    {
        m_HUD.transform.GetChild(6).gameObject.SetActive(true);
    }

    public void CloseEnemyDisplay()
    {
        m_HUD.transform.GetChild(6).gameObject.SetActive(false);
    }

    public void UpdateEnemyDisplay(Sprite image, string healthText )
    {
        m_HUD.transform.GetChild(6).GetChild(0).GetComponent<Image>().sprite = image;
        m_HUD.transform.GetChild(6).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = healthText;
    }

    public void OpenTowerDisplay()
    {
        m_HUD.transform.GetChild(7).gameObject.SetActive(true);
    }

    public void CloseTowerDisplay()
    {
        m_HUD.transform.GetChild(7).gameObject.SetActive(false);
    }

    public void UpdateTowerDisplay(Sprite image, string healthText, string damageText, string rangeText, string reloadText, string upgradeText)
    {
        GameObject towerDisplay = m_HUD.transform.GetChild(7).gameObject;
        GameObject towerStatsDisplay = towerDisplay.transform.GetChild(2).gameObject;

        towerDisplay.transform.GetChild(0).GetComponent<Image>().sprite = image;
        towerDisplay.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = healthText;

        towerStatsDisplay.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = damageText;
        towerStatsDisplay.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = rangeText;
        towerStatsDisplay.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = reloadText;
        towerStatsDisplay.transform.GetChild(7).GetComponent<TextMeshProUGUI>().text = upgradeText;
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

    public GameObject SelectedUnit
    {
        get { return m_SelectedUnit; }
        set { m_SelectedUnit = value; }
    }
}
