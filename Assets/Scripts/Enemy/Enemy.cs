using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyRank
{
    RANK_BRONZE,
    RANK_SILVER,
    RANK_GOLD,
    RANK_MAX
}

public class Enemy : MonoBehaviour
{
    public List<Material> RankMaterials;
    public List<GameObject> Waypoints;
    public GameObject SelectionHighlight;
    public Sprite ProfileSprite;

    [SerializeField] EnemyRank m_Rank;
    [SerializeField] float m_StartingHealth = 5.0f;

    LevelManager m_LevelManager;
    NavMeshAgent NavAgent;
    float m_CurrentHealth;
    int m_CurrentWaypointIndex = 0;
    int m_MoneyDropAmount = 1;
    bool m_IsDead = false;
    bool m_IsAttacking = false;

    void Start()
    {
        m_LevelManager = LevelManager.Instance;
        NavAgent = GetComponent<NavMeshAgent>();
        m_CurrentWaypointIndex = 0;
        m_StartingHealth = (m_StartingHealth + ((int)m_Rank * 5)) * m_LevelManager.WaveNum;
        m_CurrentHealth = m_StartingHealth;

        SetDropValue(m_LevelManager.WaveNum);
        InitMaterial();

        if (NavAgent && Waypoints[0] != null) 
            NavAgent.destination = Waypoints[CurrentWaypointIndex].transform.position;
    }

    // If the enemy's path is blocked, attack & destroy blocking tower to clear a path. Reset the attack once the enemy is moving again.
    private void Update()
    {
        if (GetComponent<Rigidbody>().IsSleeping() == true && m_IsAttacking == false) {
            StartCoroutine(AttackDelayTimer());
        }

        if (GetComponent<Rigidbody>().IsSleeping() == false && m_IsAttacking == true) {
            m_IsAttacking = false;
            StopCoroutine(AttackDelayTimer());
        }
    }

    // Setups up the enemy's visual appearence depending on their rank
    void InitMaterial()
    {
        if (RankMaterials == null)
            return;

        Renderer mainRenderer = GetComponent<Renderer>();
        Renderer subRenderer1 = transform.GetChild(0).GetComponent<Renderer>();
        Renderer subRenderer2 = transform.GetChild(1).GetComponent<Renderer>();

        switch (m_Rank) {
            case EnemyRank.RANK_BRONZE:
                if (mainRenderer)
                    mainRenderer.material = RankMaterials[0];
                if (subRenderer1)
                    subRenderer1.material = RankMaterials[0];
                if (subRenderer2)
                    subRenderer2.material = RankMaterials[0];
                break;

            case EnemyRank.RANK_SILVER:
                if (mainRenderer)
                    mainRenderer.material = RankMaterials[1];
                if (subRenderer1)
                    subRenderer1.material = RankMaterials[1];
                if (subRenderer2)
                    subRenderer2.material = RankMaterials[1];
                break;

            case EnemyRank.RANK_GOLD:
                if (mainRenderer)
                    mainRenderer.material = RankMaterials[2];
                if (subRenderer1)
                    subRenderer1.material = RankMaterials[2];
                if (subRenderer2)
                    subRenderer2.material = RankMaterials[2];
                break;

            case EnemyRank.RANK_MAX:
                Debug.Log("Error. Rank doesn't exist.");
                break;
        }
    }

    // Assign the amount of money the enemy drops on death, depending on the current wave number
    void SetDropValue(int waveNum)
    {
        int wave = waveNum;

        if (wave >= 0 && wave < 5)
            m_MoneyDropAmount = 1;
        else if (wave >= 5 && wave < 10)
            m_MoneyDropAmount = 2;
        else if (wave >= 10 && wave < 15)
            m_MoneyDropAmount = 3;
        else if (wave >= 15 && wave < 20)
            m_MoneyDropAmount = 4;
        else if (wave >= 20 && wave < 25)
            m_MoneyDropAmount = 5;
        else
            m_MoneyDropAmount = 6;
    }

    // Raycast infront of the enemy. If a tower is hit, damage/destroy the tower.
    void AttackTower()
    {
        m_IsAttacking = true;
        RaycastHit hit;
        int layerMask = 1 << 6;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 12.5f, ~layerMask)) {
            Debug.DrawRay(transform.position, transform.forward * 12.5f, Color.green, 1.0f);
            Tower tower = hit.transform.parent.gameObject.GetComponent<Tower>();

            if (tower && tower.CurrentHealth > 0.0f)
                tower.TakeDamage(5.0f);
        }
    }

    // Stop the life trigger timer if activated, reset the tile the enemy is on as buildable, reward + update player counters, and destroy the enemy object
    void OnDeath()
    {
        m_IsDead = true;

        LifeCounterTrigger lct = Waypoints[0].GetComponent<LifeCounterTrigger>();
        lct.StopDelayTimer(this);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, 3.0f)) {
            Tile tile = hit.transform.gameObject.GetComponent<Tile>();

            if (tile && !tile.IsSpawner) 
                tile.IsBuildable = true;
        }

        m_LevelManager.PlayerRef.m_MoneyCounter += m_MoneyDropAmount;
        m_LevelManager.UpdateMoneyCounter();
        m_LevelManager.ReduceEnemyCounter();

        Destroy(gameObject);
    }

    IEnumerator AttackDelayTimer()
    {
        yield return new WaitForSeconds(0.75f);

        AttackTower();
    }

    // When an enemy takes damange their current health is reduced. If reduced to 0 or less, the damaging tower has their attack target reset and the enemy dies.
    public void TakeDamage(float damage, Tower damagingTower)
    {
        m_CurrentHealth -= damage;

        if (m_IsDead == false && m_CurrentHealth <= 0.0f) {
            damagingTower.ResetAttackTarget();
            OnDeath();
        }
    }

    // Increase the Waypoint List's index. If the index goes over the actual amount of waypoints, reset to the start of the list.
    public void UpdateWaypointIndex()
    {
        CurrentWaypointIndex++;

        if (CurrentWaypointIndex >= Waypoints.Count)
            CurrentWaypointIndex = 0;
    }

    // Change Unity's build in Nav destination to the current waypoint's position.
    public void UpdateNavDestination()
    {
        if (NavAgent != null)
            NavAgent.destination = Waypoints[CurrentWaypointIndex].transform.position;
    }

    /* Variable Functions */
    public EnemyRank Rank
    {
        get { return m_Rank; }
        set { m_Rank = value; }
    }

    public int CurrentWaypointIndex
    {
        get { return m_CurrentWaypointIndex; }
        set { m_CurrentWaypointIndex = value; }
    }

    public float StartingHealth
    {
        get { return m_StartingHealth; }
    }

    public float CurrentHealth
    {
        get { return m_CurrentHealth; }
    }

}
