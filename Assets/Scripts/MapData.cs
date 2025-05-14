using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class CommandEntry
{
    public CommandType commandType;
    public int count;
}


[CreateAssetMenu(menuName = "Puzzle/MapData", fileName = "NewMapData")]
public class MapData : ScriptableObject
{
    public int width;
    public int height;
    public TileType[] tileTypes;
    
    public List<CommandEntry> availableCommands;

    public TileType GetTile(int x, int y)
    {
        return tileTypes[y * width + x];
    }

    public Vector2Int GetStartPosition()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (GetTile(x,y) == TileType.Start)
                {
                    return new Vector2Int(x, y);
                }
            }
        }
        return new Vector2Int(-1, -1);
    }
    
}
