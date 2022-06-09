using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowingTower : Tower
{
    // Standard tower attack raycast with the addition of adding a Reduce Movement script to the enemy if they do not have that component already.
    protected override void SpawnRaycast()
    {
        RaycastHit hit;
        Vector3 direction = (m_MuzzlePoint.transform.position + m_MuzzlePoint.transform.forward) - m_MuzzlePoint.transform.position;
        int layerMask = 1 << 7;

        if (Physics.Raycast(m_MuzzlePoint.transform.position, direction.normalized, out hit, m_Range, ~layerMask)) {
            Debug.DrawRay(m_MuzzlePoint.transform.position, direction.normalized * m_Range, Color.red, 1.0f);
            Enemy enemy = hit.transform.gameObject.GetComponent<Enemy>();

            if (enemy && enemy.CurrentHealth > 0.0f) {
                StartCoroutine(DamageDelay(enemy));

                if (enemy.GetComponent<ReduceMovement>() == null)
                    enemy.gameObject.AddComponent<ReduceMovement>();
                else
                    return;
            }
        }
    }
}
