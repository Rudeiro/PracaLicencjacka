using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using TMPro;

public class WorldArea : MonoBehaviour
{
    public ShooterAgent shooter;
    public ShooterAgent shooter2;
    [SerializeField]
    public TextMeshPro cumulativeRewardText;
    [SerializeField]
    Target targetPrefab;
    [SerializeField]
    List<SpawnArea> spawnAreas;
    private Target target;

    public int targetsCount;

    

    public void ResetArea()
    {
        //Debug.LogError("begin");
        targetsCount = 4;
        shooter.transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
        foreach (var spawnArea in spawnAreas)
        {
            spawnArea.ResetSpawnArea();
        }
    }   
    
    private void Update()
    {        
        if(targetsCount <= 0)
        {
            shooter.EndEpisode();
        }
        cumulativeRewardText.text = shooter.GetCumulativeReward().ToString("0.00");
    }
}
