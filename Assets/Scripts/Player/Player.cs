using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Camera Control variables
    [Header("Camera Controls")]
    public float panSpeed = 20.0f;
    public float panBorderThickness = 10.0f;
    public Vector2 panLimit;
    public float scrollSpeed = 20.0f;
    public Vector2 scrollLimit;

    // Player variables
    [Header("Player")]
    [SerializeField] ParticleSystem m_BuildEffect;
    public int m_MoneyCounter = 10;
    public int m_LifeCounter = 1;

    LevelManager m_LevelManager;
    GameObject m_TowerToBuild = null;
    bool m_IsReadyToBuild = false;

    private void Awake()
    {
        m_LevelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();

        if (m_BuildEffect)
            m_BuildEffect.gameObject.SetActive(false);
    }

    void Update()
    {
        UpdateCameraPosition();

        // Check if a tower is trying to be built. Place building with left click, cancel with right click
        if (m_IsReadyToBuild == true && m_TowerToBuild != null) {
            // Raycast from the mouse position downward and find position in world to spawn tower
            if (Input.GetMouseButtonDown(0)) {
                Vector3 spawnPos = Vector3.zero;
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                int layer = 1 << 1;

                Physics.Raycast(ray, out hit, 1000.0f, ~layer, QueryTriggerInteraction.Ignore);

                if (hit.collider.gameObject.tag == "Tile") {
                    spawnPos = hit.transform.position + new Vector3(0, 2.5f, 0);
                }

                if (spawnPos != Vector3.zero && hit.collider.GetComponent<Tile>().IsBuildable == true) {
                    m_MoneyCounter -= m_TowerToBuild.GetComponent<Tower>().BuildCost;
                    m_LevelManager.UpdateMoneyCounter();
                    StartCoroutine(BuildTowerTimer(spawnPos, m_TowerToBuild));
                    m_BuildEffect.gameObject.SetActive(true);
                    m_BuildEffect.gameObject.transform.position = spawnPos + new Vector3(0.0f, 2.5f, 0.0f);
                    hit.collider.GetComponent<Tile>().IsBuildable = false;
                    hit.collider.GetComponent<Tile>().IsWalkable = false;
                }

                // Reset the selected tower
                m_TowerToBuild = null;
                IsReadyToBuild = false;
            }

            // Reset the selected tower with right click
            if (Input.GetMouseButtonDown(1) && IsReadyToBuild == true) {
                m_TowerToBuild = null;
                IsReadyToBuild = false;
            }
        }
    }

    // Update the Camera's position based on received input
    void UpdateCameraPosition()
    {
        Vector3 pos = transform.position;

        if (Input.GetKey(KeyCode.W) || Input.mousePosition.y >= Screen.height - panBorderThickness) {
            pos.z += panSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S) || Input.mousePosition.y <= panBorderThickness) {
            pos.z -= panSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D) || Input.mousePosition.x >= Screen.width - panBorderThickness) {
            pos.x += panSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A) || Input.mousePosition.x <= panBorderThickness) {
            pos.x -= panSpeed * Time.deltaTime;
        }

        // Atler the zoom level of the camera based on scroll wheel input
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * scrollSpeed * 100.0f * Time.deltaTime;

        // Clamp the position changes by the limiters , then update the camera's position
        pos.x = Mathf.Clamp(pos.x, panLimit.x * 0.5f, panLimit.x);
        pos.y = Mathf.Clamp(pos.y, scrollLimit.x, scrollLimit.y);
        pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y * 0.5f);

        transform.position = pos;
    }

    // Deduct the cost of the tower from the player, and spawn the selected tower at desired position and rotation
    void SpawnTower(Vector3 spawnPos, GameObject towerPrefab)
    {
        Instantiate(towerPrefab, spawnPos, Quaternion.identity);
    }

    IEnumerator BuildTowerTimer(Vector3 spawnPos, GameObject towerPrefab)
    {
        yield return new WaitForSeconds((float)m_TowerToBuild.GetComponent<Tower>().BuildTime);

        m_BuildEffect.gameObject.SetActive(false);
        SpawnTower(spawnPos, towerPrefab);
    }

    public void SetTowerToBuild(GameObject towerPrefab)
    {
        m_TowerToBuild = towerPrefab;
    }

    public bool IsReadyToBuild
    {
        get { return m_IsReadyToBuild; }
        set { m_IsReadyToBuild = value; }
    }
}
