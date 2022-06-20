using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ReduceMovement : MonoBehaviour
{
    NavMeshAgent m_NavAgent;
    float m_OriginalSpeed, m_ReducedSpeed, m_Duration;

    // Grab specific enemy data to fill necessary values, set the reduced movement, and start the reset timer
    void Start()
    {
        m_NavAgent = GetComponent<NavMeshAgent>();
        m_OriginalSpeed = m_NavAgent.speed;
        m_ReducedSpeed = m_OriginalSpeed * 0.5f;
        m_Duration = 8.0f;
        m_NavAgent.speed = m_ReducedSpeed;

        StartCoroutine(ResetSpeed());
    }

    // Return the enemy speed to the original value after the duration is over. Then destroy this script/component
    IEnumerator ResetSpeed()
    {
        yield return new WaitForSeconds(m_Duration);

        m_NavAgent.speed = m_OriginalSpeed;
        Destroy(this);
    }

    public float Duration
    {
        get { return m_Duration; }
        set { m_Duration = value; }
    }
}
