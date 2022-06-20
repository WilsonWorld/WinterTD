using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMenu : MonoBehaviour
{
    [SerializeField] AudioSource m_sfxSource;
    [SerializeField] GameObject BasicTowerPrefab;
    [SerializeField] GameObject BlitzTowerPrefab;
    [SerializeField] GameObject BlastTowerPrefab;
    [SerializeField] GameObject SlowTowerPrefab;
    Player m_playerRef;

    // Start is called before the first frame update
    void Start()
    {
        m_playerRef = Camera.main.GetComponent<Player>();
    }

    public void OnSlowTowerClick()
    {
        if (SlowTowerPrefab == null)
            return;

        if (m_playerRef.m_MoneyCounter < SlowTowerPrefab.GetComponent<Tower>().BuildCost)
            return;

        m_playerRef.SetTowerToBuild(SlowTowerPrefab);
        m_playerRef.IsReadyToBuild = true;
    }

    public void OnBlastTowerClick()
    {
        if (BlastTowerPrefab == null)
            return;

        if (m_playerRef.m_MoneyCounter < BlastTowerPrefab.GetComponent<Tower>().BuildCost)
            return;

        m_playerRef.SetTowerToBuild(BlastTowerPrefab);
        m_playerRef.IsReadyToBuild = true;
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
