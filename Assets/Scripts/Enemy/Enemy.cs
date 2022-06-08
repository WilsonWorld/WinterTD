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

    [SerializeField] EnemyRank m_Rank;
    [SerializeField] float m_StartingHealth = 5.0f;
    [SerializeField] int m_MoneyDropAmount = 1;

    LevelManager m_LevelManager;
    NavMeshAgent NavAgent;
    float m_CurrentHealth;
    int m_CurrentWaypointIndex;
    bool m_AttackTower = false;

    void Start()
    {
        m_LevelManager = LevelManager.Instance;
        NavAgent = GetComponent<NavMeshAgent>();
        m_CurrentWaypointIndex = 0;
        m_CurrentHealth = (m_StartingHealth + ((int)m_Rank * 5)) * m_LevelManager.WaveNum;

        InitMaterial();

        if (NavAgent && Waypoints[0] != null) 
            NavAgent.destination = Waypoints[CurrentWaypointIndex].transform.position;
    }

    private void Update()
    {
        if (GetComponent<Rigidbody>().IsSleeping() == true && m_AttackTower == false)
        {
            m_AttackTower = true;
            Tower nearTower = null;
            float nearestTower = float.MaxValue;
            Collider[] colliders = Physics.OverlapSphere(transform.position, 20.0f);

            foreach (Collider towerObj in colliders)
            {
                Tower hitTower = towerObj.GetComponent<Tower>();

                if (hitTower) {
                    float dist = Vector3.Distance(this.transform.position, hitTower.transform.position);

                    if (dist < nearestTower) {
                        nearestTower = dist;
                        nearTower = hitTower;
                    }
                }
            }

            if (nearTower) {
                nearTower.TakeDamage(5.0f);
                m_AttackTower = false;
            }
        }
    }

    // Setups up the enemy's visual appearence depending on their rank
    void InitMaterial()
    {
        if (RankMaterials == null)
            return;

        switch (m_Rank)
        {
            case EnemyRank.RANK_BRONZE:
                if (GetComponent<Renderer>())
                    GetComponent<Renderer>().material = RankMaterials[0];

                if (transform.GetChild(0).GetComponent<Renderer>())
                    transform.GetChild(0).GetComponent<Renderer>().material = RankMaterials[0];

                if (transform.GetChild(1).GetComponent<Renderer>())
                    transform.GetChild(1).GetComponent<Renderer>().material = RankMaterials[0];
                break;
            case EnemyRank.RANK_SILVER:
                if (GetComponent<Renderer>())
                    GetComponent<Renderer>().material = RankMaterials[1];

                if (transform.GetChild(0).GetComponent<Renderer>())
                    transform.GetChild(0).GetComponent<Renderer>().material = RankMaterials[1];

                if (transform.GetChild(1).GetComponent<Renderer>())
                    transform.GetChild(1).GetComponent<Renderer>().material = RankMaterials[1];
                break;
            case EnemyRank.RANK_GOLD:
                if (GetComponent<Renderer>())
                    GetComponent<Renderer>().material = RankMaterials[2];

                if (transform.GetChild(0).GetComponent<Renderer>())
                    transform.GetChild(0).GetComponent<Renderer>().material = RankMaterials[2];

                if (transform.GetChild(1).GetComponent<Renderer>())
                    transform.GetChild(1).GetComponent<Renderer>().material = RankMaterials[2];
                break;
            case EnemyRank.RANK_MAX:
                Debug.Log("Error. Rank doesn't exist.");
                break;
        }
    }

    // When an enemy dies increase the player's money, update the releveant UI elements, sets the tile back to buildable, and destroys the game object
    void OnDeath()
    {
        LifeCounterTrigger lct = Waypoints[0].GetComponent<LifeCounterTrigger>();
        lct.StopDelayTimer(this);

        m_LevelManager.PlayerRef.m_MoneyCounter += m_MoneyDropAmount;
        m_LevelManager.UpdateMoneyCounter();
        m_LevelManager.ReduceEnemyCounter();

        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, 3.0f)) {

            Tile tile = hit.transform.gameObject.GetComponent<Tile>();
            if (tile && !tile.IsSpawner) {
                tile.IsBuildable = true;
            }
        }

        Destroy(gameObject);
    }

    // When an enemy takes damange their current health is reduced. If reduced to 0 or less, the enemy dies.
    public void TakeDamage(float damage, Tower damagingTower)
    {
        m_CurrentHealth -= damage;

        if (m_CurrentHealth <= 0.0f) {
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
    public int CurrentWaypointIndex
    {
        get { return m_CurrentWaypointIndex; }
        set { m_CurrentWaypointIndex = value; }
    }

    public float CurrentHealth
    {
        get { return m_CurrentHealth; }
    }

    public EnemyRank Rank
    {
        get { return m_Rank; }
        set { m_Rank = value; }
    }
}
