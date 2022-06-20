using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastTower : Tower
{
    [SerializeField] ParticleSystem m_ProjectileBlastPrefab2 = null;
    [SerializeField] ParticleSystem m_ProjectileBlastPrefab3 = null;

    // Override the standard AttackNearestTarget function to contain/play multiple PFX instances
    protected override void AttackNearestTarget()
    {
        if (m_CanShoot == false)
            return;

        m_CanShoot = false;
        m_ProjectileBlastPrefab.Play();
        m_ProjectileBlastPrefab2.Play();

        if (m_IsUpgraded == true)
            m_ProjectileBlastPrefab3.Play();

        m_sfxSource.PlayOneShot(m_BlastClip);
        SpawnRaycast();
        StartCoroutine(CooldownTimer());
    }
}
