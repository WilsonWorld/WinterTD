using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMenu : MonoBehaviour
{
    [SerializeField] GameObject BasicTowerPrefab;
    [SerializeField] GameObject BlitzTowerPrefab;
    Player m_playerRef;

    // Start is called before the first frame update
    void Start()
    {
        m_playerRef = GameObject.Find("Main Camera").GetComponent<Player>();
    }

    public void OnBlitzTowerClick()
    {
        if (BlitzTowerPrefab == null)
            return;

        if (m_playerRef.m_MoneyCounter < BlitzTowerPrefab.GetComponent<Tower>().BuildCost)
            return;

        m_playerRef.SetTowerToBuild(BlitzTowerPrefab);
        m_playerRef.IsReadyToBuild = true;
    }

    public void OnBasicTowerClick()
    {
        if (BasicTowerPrefab == null)
            return;

        if (m_playerRef.m_MoneyCounter < BasicTowerPrefab.GetComponent<Tower>().BuildCost)
            return;

        m_playerRef.SetTowerToBuild(BasicTowerPrefab);
        m_playerRef.IsReadyToBuild = true;
    }
}
