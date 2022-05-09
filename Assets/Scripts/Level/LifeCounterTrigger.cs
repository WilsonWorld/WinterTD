using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeCounterTrigger : MonoBehaviour
{
    LevelManager m_LevelManager;
    Player m_playerRef;

    void Start()
    {
        m_LevelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        m_playerRef = Camera.main.GetComponent<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.gameObject.GetComponent<Enemy>();

        if (enemy) {
            StartCoroutine(DelayTimer(enemy));
        }
    }

    // Dcrement the player's LifeCount and update the UI elements. If the LifeCounter is 0 or less, the game is over and the player has been defeated
    void ReduceLifeCounter()
    {
        if (m_playerRef == null)
            return;

        m_playerRef.m_LifeCounter--;
        m_LevelManager.UpdateLifeCounter();
        m_LevelManager.ReduceEnemyCounter();

        if (m_playerRef.m_LifeCounter <= 0) {
            m_LevelManager.IsGameOver = true;
            ClearEnemies();
            m_LevelManager.OpenDefeatScreen();
        }
    }

    // Find any & all remaining enemy objects then remove them from the level.
    void ClearEnemies()
    {
        GameObject[] remainingEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (remainingEnemies != null) {
            foreach (var enemy in remainingEnemies){
                Destroy(enemy);
            }
        }
    }

    IEnumerator DelayTimer(Enemy enemy)
    {
        yield return new WaitForSeconds(2.0f);

        if (enemy) {
            ReduceLifeCounter();
            Destroy(enemy.gameObject);
        }
    }

    public void StopDelayTimer(Enemy enemy)
    {
        StopCoroutine(DelayTimer(enemy));
    }
}
