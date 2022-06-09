using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastTower : Tower
{
    [SerializeField] ParticleSystem m_ProjectileBlastPrefab2;

    // Override the standard AttackNearestTarget function to contain/play multiple PFX instances
    protected override void AttackNearestTarget()
    {
        if (m_CanShoot == true) {
            m_CanShoot = false;
            m_ProjectileBlastPrefab.Play();
            m_ProjectileBlastPrefab2.Play();
            SpawnRaycast();
            StartCoroutine(CooldownTimer());
        }
    }
}
