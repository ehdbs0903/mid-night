using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("맵 관련")]
    public MapData[] mapDatas;
    public MapManager mapManager;
    public GameObject playerPrefab;

    [Header("UI 관련")] 
    public Transform commandPanelParent;

    [System.Serializable]
    public class CommandBlockPrefab
    {
        public CommandType type;
        public GameObject prefab;
    }
    public List<CommandBlockPrefab> commandBlockPrefabs;
    
    private GameObject PlayerInstance;
    private void Awake()
    {
        int index = PlayerPrefs.GetInt("SelectedStage", 0);
        if (index < 0 || index >= mapDatas.Length)
        {
            Debug.LogError("잘못된 StageIndex");
            return;
        }
        
        mapManager.mapData = mapDatas[index];
        mapManager.Generate();
        
        SpawnPlayer(mapManager.mapData);
        
        GenerateCommandBlocks(mapManager.mapData.availableCommands);
    }

    private void SpawnPlayer(MapData mapData)
    {
        Vector2Int startPos = mapData.GetStartPosition();
        if (startPos.x == -1) return;
        
        Vector3 worldPos = new Vector3(startPos.x, 1f, startPos.y);
        PlayerInstance = Instantiate(playerPrefab, worldPos, Quaternion.identity);
    }

    private void GenerateCommandBlocks(List<CommandEntry> commands)
    {
        foreach (Transform child in commandPanelParent)
        {
            Destroy(child.gameObject);
        }
        
        foreach (var entry in commands)
        {
                GameObject blockPrefab = GetBlockPrefab(entry.commandType);
                if (blockPrefab == null)
                {
                    Debug.LogWarning($"missing block prefab: {entry.commandType}");
                    continue;
                }

                for (int i = 0; i < entry.count; i++)
                {
                    GameObject block = Instantiate(blockPrefab, commandPanelParent);
                    
                    CommandBlock cb = block.GetComponent<CommandBlock>();
                    if (cb != null);
                    {
                        cb.commandType = entry.commandType;
                    }
                }
        }
    }

    private GameObject GetBlockPrefab(CommandType type)
    {
        foreach (var entry in commandBlockPrefabs)
        {
            if (entry.type == type)
                return entry.prefab;
        }
        return null;
    }
}
