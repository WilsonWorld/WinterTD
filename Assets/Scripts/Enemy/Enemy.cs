using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public List<GameObject> Waypoints;

    [SerializeField] float m_StartingHealth = 5.0f;
    [SerializeField] int m_MoneyDropAmount = 1;

    LevelManager m_LevelManager;
    NavMeshAgent NavAgent;
    float m_MaxHealth;
    float m_CurrentHealth;
    int m_CurrentWaypointIndex;


    void Awake()
    {
        m_LevelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        NavAgent = GetComponent<NavMeshAgent>();
        m_CurrentWaypointIndex = 0;

        m_MaxHealth = m_StartingHealth + m_LevelManager.WaveNum;
        m_CurrentHealth = m_MaxHealth;

        if (NavAgent && Waypoints[0] != null) {
            NavAgent.destination = Waypoints[CurrentWaypointIndex].transform.position;
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
            if (tile) {
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

    public int CurrentWaypointIndex
    {
        get { return m_CurrentWaypointIndex; }
        set { m_CurrentWaypointIndex = value; }
    }

    public float CurrentHealth
    {
        get { return m_CurrentHealth; }
    }
}
