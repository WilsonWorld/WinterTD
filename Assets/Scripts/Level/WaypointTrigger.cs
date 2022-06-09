using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointTrigger : MonoBehaviour
{
    // Check if objects entering the trigger zone have an enemy script, if so, move them to the next waypoint
    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.gameObject.GetComponent<Enemy>();

        if (enemy)
            StartCoroutine(NavUpdateTimer(enemy));
    }

    IEnumerator NavUpdateTimer(Enemy enemy)
    {
        yield return new WaitForSeconds(1.0f);

        enemy.UpdateWaypointIndex();
        enemy.UpdateNavDestination();
    }
}
