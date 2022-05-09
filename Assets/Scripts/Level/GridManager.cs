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

    void GenerateGrid()
    {
        m_Tiles = new Dictionary<Vector3, Tile>();

        for (int z = 0; z < m_Height; z++) {
            for (int x= 0; x < m_Width; x++) {
                int index = x + (z * m_Width);
                var spawnedTile = Instantiate(m_TilePrefab, new Vector3(x * m_TilePrefab.TileSize, 0.0f, z * m_TilePrefab.TileSize), Quaternion.identity);
                spawnedTile.name = "Tile " + index.ToString();
                spawnedTile.transform.SetParent(gameObject.transform);

                var isOffset = (x % 2 == 0 && z % 2 != 0) || (x % 2 != 0 && z % 2 == 0);
                spawnedTile.Init(isOffset);

                m_Tiles[new Vector3(x * m_TilePrefab.TileSize, 2.5f, z * m_TilePrefab.TileSize)] = spawnedTile;
            }
        }
    }

    public Tile GetTileAtPos(Vector3 pos)
    {
        if (m_Tiles.TryGetValue(pos, out var tile)) {
            return tile;
        }
        else {
            return null;
        }
    }
}
