using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool IsWalkable = false;
    public bool IsFlyable = false;
    public bool IsBuildable = false;
    [HideInInspector] public int TileSize = 5;

    [SerializeField] private MeshRenderer m_Renderer;
    [SerializeField] private Color m_BaseColor;
    [SerializeField] private Color m_OffsetColor;
    [SerializeField] private GameObject m_Highlight;
    [SerializeField] private Color m_BuildableColor;
    [SerializeField] private Color m_NonBuildableColor;

    Player m_PlayerRef;

    // Start is called before the first frame update
    void Start()
    {
        m_PlayerRef = Camera.main.GetComponent<Player>();
        Physics.queriesHitTriggers = false;
    }

    public void Init(bool isOffset)
    {
        m_Renderer.material.color = isOffset ? m_OffsetColor : m_BaseColor;
    }

    private void OnMouseEnter()
    {
        if (m_PlayerRef.IsReadyToBuild == false)
            return;

        m_Highlight.SetActive(true);

        if (IsBuildable == true)
            m_Highlight.GetComponent<MeshRenderer>().material.color = m_BuildableColor;
        else
            m_Highlight.GetComponent<MeshRenderer>().material.color = m_NonBuildableColor;
    }

    private void OnMouseExit()
    {
        m_Highlight.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.tag == "Enemy")
            IsBuildable = false;
    }

    private void OnCollisionExit(Collision collision)
    {
        IsBuildable = true;
    }
}
