using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField]
    List<SpawnArea> spawnAreas;
    [SerializeField]
    WorldArea worldArea;
    
    private int targetsCount;

    public WorldArea WorldArea { get { return worldArea; } }
    public int TargetsCount { get { return targetsCount; } }

    void Start()
    {
        ResetRoom();
    }

    public void ResetRoom()
    {
        targetsCount = 0;
        foreach (var spawnArea in spawnAreas)
        {
            spawnArea.ResetSpawnArea();
            targetsCount += spawnArea.TargetsCount;
        }        
    }
    
}
