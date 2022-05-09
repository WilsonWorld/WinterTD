using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMenu : MonoBehaviour
{
    [SerializeField] GameObject BasicTowerPrefab;
    Player m_playerRef;

    // Start is called before the first frame update
    void Start()
    {
        m_playerRef = GameObject.Find("Main Camera").GetComponent<Player>();
    }

    public void OnBasicTowerClick()
    {
        m_playerRef.SetTowerToBuild(BasicTowerPrefab);
        m_playerRef.IsReadyToBuild = true;
    }
}
