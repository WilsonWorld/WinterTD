using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
    public int m_MoneyCounter = 10;
    public int m_LifeCounter = 1;

    [Header("Prefabs")]
    [SerializeField] GameObject UBasicTowerPrefab;
    [SerializeField] GameObject UBlitzTowerPrefab;
    [SerializeField] GameObject UBlastTowerPrefab;
    [SerializeField] GameObject USlowTowerPrefab;

    LevelManager m_LevelManager;
    GameObject m_TowerToBuild = null;
    bool m_IsReadyToBuild = false;

    private void Awake()
    {
        m_LevelManager = LevelManager.Instance;
    }

    void Update()
    {
        UpdateCameraPosition();

        // Check if the player is trying to select a unit when left clicking their mouse, if they are not placing a tower to build. 
        if (m_IsReadyToBuild == false) {
            if (Input.GetMouseButtonDown(0)) {
                var pointerEventData = new PointerEventData(EventSystem.current);
                pointerEventData.position = Input.mousePosition;
                var raycastResults = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerEventData, raycastResults);

                if (raycastResults.Count > 0) {
                    foreach (var result in raycastResults) {
                        if (result.gameObject.tag == "UI")
                            return;
                    }
                }

                ClearSelectedUnit();
                SelectUnit();
            }
        }

        // Check if a tower is trying to be built. Place building with left click, cancel with right click
        if (m_IsReadyToBuild == true && m_TowerToBuild != null) {
            if (Input.GetMouseButtonDown(0))
                PlaceTower();

            if (Input.GetMouseButtonDown(1) && IsReadyToBuild == true)
                ResetBuild();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            m_LevelManager.OpenPauseMenu();
    }

    // Update the Camera's position based on received input
    void UpdateCameraPosition()
    {
        Vector3 pos = transform.position;

        if (Input.GetKey(KeyCode.W) || Input.mousePosition.y >= Screen.height - panBorderThickness)
            pos.z += panSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.S) || Input.mousePosition.y <= panBorderThickness)
            pos.z -= panSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.D) || Input.mousePosition.x >= Screen.width - panBorderThickness)
            pos.x += panSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.A) || Input.mousePosition.x <= panBorderThickness)
            pos.x -= panSpeed * Time.deltaTime;

        // Atler the zoom level of the camera based on scroll wheel input
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * scrollSpeed * 100.0f * Time.deltaTime;

        // Clamp the position changes by the limiters , then update the camera's position
        pos.x = Mathf.Clamp(pos.x, panLimit.x * 0.5f, panLimit.x);
        pos.y = Mathf.Clamp(pos.y, scrollLimit.x, scrollLimit.y);
        pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y * 0.5f);

        transform.position = pos;
    }

    // Handles the logic for activating the selection UI and visual object attached to the gameobject if the raycast hits that object.
    void SelectUnit()
    {
        GameObject hitObject = MouseRaycast();

        if (hitObject.tag == "Enemy") {
            Enemy hitEnemy = hitObject.GetComponent<Enemy>();
            string healthText = hitEnemy.CurrentHealth.ToString() + " / " + hitEnemy.StartingHealth.ToString();
            hitEnemy.SelectionHighlight.SetActive(true);

            m_LevelManager.SelectedUnit = hitEnemy.gameObject;
            m_LevelManager.OpenEnemyDisplay();
            m_LevelManager.UpdateEnemyDisplay(hitEnemy.ProfileSprite, healthText);
        }

        if (hitObject.tag == "Tower") {
            Tower hitTower = hitObject.transform.parent.gameObject.GetComponent<Tower>();
            string healthText = hitTower.CurrentHealth.ToString() + " / " + hitTower.StartingHealth.ToString();
            string damageText = hitTower.DamageMin.ToString() + "-" + hitTower.DamageMax.ToString();
            string rangeText = hitTower.Range.ToString() + "m";
            string reloadText = hitTower.Cooldown.ToString() + "s";
            hitTower.SelectionHighlight.SetActive(true);

            m_LevelManager.SelectedUnit = hitTower.gameObject;
            m_LevelManager.OpenTowerDisplay();
            m_LevelManager.UpdateTowerDisplay(hitTower.ProfileSprite, healthText, damageText, rangeText, reloadText);
        }
    }

    // Closes unit displays UI and resets the selected unit
    void ClearSelectedUnit()
    {
        if (m_LevelManager.SelectedUnit == null)
            return;

        if (m_LevelManager.SelectedUnit.GetComponent<Enemy>() != null)
            m_LevelManager.SelectedUnit.GetComponent<Enemy>().SelectionHighlight.SetActive(false);
        else
            m_LevelManager.SelectedUnit.GetComponent<Tower>().SelectionHighlight.SetActive(false);

        m_LevelManager.CloseEnemyDisplay();
        m_LevelManager.CloseTowerDisplay();
        m_LevelManager.SelectedUnit = null;
    }

    // Raycast from the mouse pos to the game world. Returns the object it hits
    GameObject MouseRaycast()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layer = 1 << 1;
        Physics.Raycast(ray, out hit, 1000.0f, ~layer, QueryTriggerInteraction.Ignore);

        return hit.collider.gameObject;
    }

    // Check if the player can place a tower on a tile. If possible and the player can afford the tower, the tower is placed on top of the tile.
    void PlaceTower()
    {
        GameObject hitObject = MouseRaycast();
        Vector3 spawnPos = Vector3.zero;

        if (hitObject.tag == "Tile")
            spawnPos = hitObject.transform.position + new Vector3(0, 2.5f, 0);

        // Reduce the player's money for the build cost, spawn in a tower, and set the tile it's on to un-buildable/walkable
        if (spawnPos != Vector3.zero && hitObject.GetComponent<Tile>().IsBuildable == true) {
            m_MoneyCounter -= m_TowerToBuild.GetComponent<Tower>().BuildCost;
            m_LevelManager.UpdateMoneyCounter();
            SpawnTower(spawnPos, m_TowerToBuild);
            hitObject.GetComponent<Tile>().IsBuildable = false;
            hitObject.GetComponent<Tile>().IsWalkable = false;
        }

        ResetBuild();
    }

    // Reset build variables
    void ResetBuild()
    {
        m_TowerToBuild = null;
        IsReadyToBuild = false;
    }

    // Deduct the cost of the tower from the player, and spawn the selected tower at desired position and rotation
    void SpawnTower(Vector3 spawnPos, GameObject towerPrefab)
    {
        Instantiate(towerPrefab, spawnPos, Quaternion.identity);
    }

    // Sell the selected tower, close the selected tower display UI, and reset the selected unit
    public void SellSelectedTower()
    {
        Tower selectedTower = m_LevelManager.SelectedUnit.GetComponent<Tower>();
        if (selectedTower == null)
            return;

        selectedTower.SellTower();
        m_LevelManager.CloseTowerDisplay();
        m_LevelManager.SelectedUnit = null;
    }

    // Upgrades the selected tower to a more powerful version, if the player can afford the upgrade cost
    public void UpgradeSelectedTower()
    {
        Tower selectedTower = m_LevelManager.SelectedUnit.GetComponent<Tower>();
        if (selectedTower == null)
            return;

        // Check if the selected tower can be stored in one of the tower sub types. If not, the upgrade is for a basic tower.
        FastTower fastTower = selectedTower.gameObject.GetComponent<FastTower>();
        SlowingTower slowTower = selectedTower.gameObject.GetComponent<SlowingTower>();
        SplashTower splashTower = selectedTower.gameObject.GetComponent<SplashTower>();

        if (fastTower)
            SetTowerToBuild(UBlitzTowerPrefab);
        else if (slowTower)
            SetTowerToBuild(USlowTowerPrefab);
        else if (splashTower)
            SetTowerToBuild(UBlastTowerPrefab);
        else
            SetTowerToBuild(UBasicTowerPrefab);

        // Reduce the player's money by the build cost if they can afford it, otherwise reset the build que and exit the function
        int cost = m_TowerToBuild.GetComponent<Tower>().BuildCost;

        if (m_MoneyCounter >= cost) {
            m_MoneyCounter -= cost;
            m_LevelManager.UpdateMoneyCounter();
        }
        else {
            SetTowerToBuild(null);
            return;
        }

        // Save the position of the old tower before destroying it and spawning in the upgraded tower
        Vector3 towerPos = selectedTower.gameObject.transform.position;

        Destroy(selectedTower.gameObject);
        SpawnTower(towerPos, m_TowerToBuild);

        // Reset the selection UI and clear the build que
        m_LevelManager.CloseTowerDisplay();
        m_LevelManager.SelectedUnit = null;
        m_TowerToBuild = null;
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
