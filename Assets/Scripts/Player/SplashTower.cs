using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashTower : Tower
{
    float m_overlapRadius = 7.5f;

    // Override the base function to instead do a spherecast at the original raycast's impact point, damaging each enemy inside the sphere's radius
    protected override void SpawnRaycast()
    {
        RaycastHit hit;
        Vector3 direction = (m_MuzzlePoint.transform.position + m_MuzzlePoint.transform.forward) - m_MuzzlePoint.transform.position;
        int layerMask = 1 << 7;

        if (Physics.Raycast(m_MuzzlePoint.transform.position, direction.normalized, out hit, m_Range, ~layerMask)) {
            Enemy enemy = hit.transform.gameObject.GetComponent<Enemy>();

            if (enemy) {
                int damage = GenerateRandomDamange(m_DamageMin, m_DamageMax);
                Collider[] colliders = Physics.OverlapSphere(enemy.transform.position, m_overlapRadius);

                foreach (Collider enemyObj in colliders) {
                    Enemy hitEnemy = enemyObj.GetComponent<Enemy>();

                    if (hitEnemy && hitEnemy.CurrentHealth > 0.0f)
                        StartCoroutine(DamageDelay(hitEnemy));
                }
            }
        }
    }
}
