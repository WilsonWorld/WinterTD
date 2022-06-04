using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] protected ParticleSystem m_ProjectileBlastPrefab;
    [SerializeField] protected ParticleSystem m_BuildEffect;
    [SerializeField] protected float m_Cooldown = 5.0f;
    [SerializeField] protected float m_Range = 60.0f;
    [SerializeField] protected int m_DamageMin = 1;
    [SerializeField] protected int m_DamageMax = 3;
    [SerializeField] protected int m_BuildCost = 5;
    [SerializeField] protected int m_BuildTime = 5;

    protected GameObject m_AttackTarget;
    protected GameObject m_MainTower;
    protected Transform m_MuzzlePoint;
    protected bool m_CanShoot = true;

    Quaternion targetRotation = Quaternion.identity;
    float turretRotSpeed = 90.0f;


    void Start()
    {
        m_MainTower = transform.GetChild(0).gameObject;
        m_MuzzlePoint = transform.GetChild(0).GetChild(0).GetChild(0);
        m_BuildEffect = transform.GetChild(2).GetComponent<ParticleSystem>();

        // Start with the turret deactivated, until the tower has been built
        m_MainTower.gameObject.SetActive(false);

        // Starts building the tower once created.
        StartCoroutine(BuildTowerTimer());
    }


    void Update()
    {
        if (m_MainTower == null)
            return;

        // Face Towards the target if there is one
        if (m_AttackTarget != null) {
            Vector3 targetDir = (m_AttackTarget.transform.position - m_MainTower.transform.position).normalized;
            targetRotation = Quaternion.LookRotation(targetDir);
            m_MainTower.transform.rotation = Quaternion.RotateTowards(m_MainTower.transform.rotation, targetRotation, turretRotSpeed * Time.deltaTime);
            AttackNearestTarget();
        }
    }

    // If there are any enemies inside the detection sphere, tower will target them, look towards the target, and attack
    private void OnTriggerStay(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();

        if (enemy == null || m_AttackTarget != null)
            return;

        FindNearestTarget();
    }

    // If enemies go out of attack range, switch to the next closest enemy to attack
    private void OnTriggerExit(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();

        if (enemy == null)
            return;

        ResetAttackTarget();
    }

    // Check for other colliders with a spherical radius, checking for the closest enemy to set as a target
    protected void FindNearestTarget()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, m_Range);
        GameObject nearestObject = null;
        float nearestCollider = Mathf.Infinity;

        foreach (var collider in hitColliders) {
            if (collider.gameObject.tag == "Enemy") {
                float distanceToCollider = Vector3.Distance(collider.transform.position, transform.position);

                if (distanceToCollider < nearestCollider) {
                    nearestCollider = distanceToCollider;
                    nearestObject = collider.gameObject;
                }
            }
        }
        AttackTarget = nearestObject;
    }

    // Uses the tower's attack and attack effects, futher shooting is prevented until the cooldown timer is finsihed.
    protected virtual void AttackNearestTarget()
    {
        if (m_CanShoot == true) {
            m_CanShoot = false;
            m_ProjectileBlastPrefab.Play();
            SpawnRaycast();
            StartCoroutine(CooldownTimer());
        }
    }

    // Fires off a raycast from the muzzle point forward. If it hits an enemy, random amount of damage between the max and min will be calculated and applied to the enemy if their health is greater than 0.
    protected virtual void SpawnRaycast()
    {
        RaycastHit hit;
        Vector3 direction = (m_MuzzlePoint.transform.position + m_MuzzlePoint.transform.forward) - m_MuzzlePoint.transform.position;
        int layerMask = 1 << 7;

        if (Physics.Raycast(m_MuzzlePoint.transform.position, direction.normalized, out hit, m_Range, ~layerMask)) {
            Debug.DrawRay(m_MuzzlePoint.transform.position, direction.normalized * m_Range, Color.red, 1.0f);
            Enemy enemy = hit.transform.gameObject.GetComponent<Enemy>();

            if (enemy && enemy.CurrentHealth > 0.0f) {
                int damage = GenerateRandomDamange(m_DamageMin, m_DamageMax);
                enemy.TakeDamage(damage, this);
            }
        }
    }

    protected int GenerateRandomDamange(int min, int max)
    {
        int randomDamage = Random.Range(min, max + 1);
        return randomDamage;
    }

    // Wait the cooldown duration before letting towers fire again.
    protected IEnumerator CooldownTimer()
    {
        yield return new WaitForSeconds(m_Cooldown);
        m_CanShoot = true;
    }

    // Wait until the cooldown finishes before stopping the build efffect and activating the turret
    IEnumerator BuildTowerTimer()
    {
        yield return new WaitForSeconds(BuildTime);

        m_BuildEffect.gameObject.SetActive(false);
        m_MainTower.gameObject.SetActive(true);
    }

    public void ResetAttackTarget()
    {
        m_AttackTarget = null;
        FindNearestTarget();
    }

    public GameObject AttackTarget
    {
        get { return m_AttackTarget; }
        set { m_AttackTarget = value; }
    }

    public int BuildCost
    {
        get { return m_BuildCost; }
        set { m_BuildCost = value; }
    }

    public int BuildTime
    {
        get { return m_BuildTime; }
        set { m_BuildTime = value; }
    }
}
