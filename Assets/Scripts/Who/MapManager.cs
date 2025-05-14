using System;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject baseFloorPrefab;
    public GameObject wallPrefab;
    public GameObject redWallPrefab;
    public GameObject blueWallPrefab;
    public GameObject goalPrefab;
    
    [HideInInspector]public MapData mapData;

    public Transform floorParent;
    public Transform objectParent;
    
    public void Generate()
    {
        GenerateBaseFloor();
        GenerateObjects();
    }

    private void GenerateBaseFloor()
    {
        for (int x = 0; x < mapData.width; x++)
        {
            for (int y = 0; y < mapData.height; y++)
            {
                Vector3 pos = new Vector3(x, 0f, y);
                Instantiate(baseFloorPrefab, pos, Quaternion.identity, floorParent);
            }
        }
    }

    private void GenerateObjects()
    {
        for (int x = 0; x < mapData.width; x++)
        {
            for (int y = 0; y < mapData.height; y++)
            {
                TileType type = mapData.GetTile(x, y);
                if (type == TileType.Floor || type == TileType.Start) continue;

                GameObject prefabToSpawn = GetPrefabByType(type);
                if (prefabToSpawn == null) continue;

                // Goal만 따로 높이와 회전 조정
                Vector3 pos = (type == TileType.Goal)
                    ? new Vector3(x, 0.55f, y)
                    : new Vector3(x, 1f, y);

                Quaternion rotation = (type == TileType.Goal)
                    ? Quaternion.Euler(-90f, 0f, 0f)
                    : Quaternion.identity;

                Instantiate(prefabToSpawn, pos, rotation, objectParent);
            }
        }
    }

    GameObject GetPrefabByType(TileType type)
    {
        switch (type)
        {
            case TileType.Wall: return wallPrefab;
            case TileType.RedWall: return redWallPrefab;
            case TileType.BlueWall: return blueWallPrefab;
            case TileType.Goal: return goalPrefab;
            default: return null;
        }
    }
}
