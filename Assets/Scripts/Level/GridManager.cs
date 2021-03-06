using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Tile m_TilePrefab;
    [SerializeField] private int m_Width;
    [SerializeField] private int m_Height;

    private Dictionary<Vector3, Tile> m_Tiles;

    void Start()
    {
        GenerateGrid();
    }

    // Create a grid of tiles and initialize them.
    public void GenerateGrid()
    {
        m_Tiles = new Dictionary<Vector3, Tile>();

        for (int z = 0; z < m_Height; z++) {
            for (int x= 0; x < m_Width; x++) {
                int index = x + (z * m_Width);
                var spawnedTile = Instantiate(m_TilePrefab, new Vector3(x * m_TilePrefab.TileSize, 0.0f, z * m_TilePrefab.TileSize), Quaternion.identity);
                spawnedTile.name = "Tile " + index.ToString();
                spawnedTile.transform.SetParent(gameObject.transform);

                m_Tiles[new Vector3(x * m_TilePrefab.TileSize, 2.5f, z * m_TilePrefab.TileSize)] = spawnedTile;
            }
        }
    }

    public void ClearGrid()
    {
        m_Tiles.Clear();
    }

    // Return a grid's tile at the world pos specificed. 
    public Tile GetTileAtPos(Vector3 pos)
    {
        if (m_Tiles.TryGetValue(pos, out var tile))
            return tile;
        else 
            return null;
    }
}
