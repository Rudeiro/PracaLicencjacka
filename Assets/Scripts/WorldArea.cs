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
    public TextMeshPro cumulativeRewardText2;
    [SerializeField]
    public TextMeshPro cumulativeRewardTextRew;
    [SerializeField]
    public TextMeshPro cumulativeRewardTextRew2;
    [SerializeField]
    Target targetPrefab;
    [SerializeField]
    List<SpawnArea> spawnAreas;
    [SerializeField]
    bool selfPlay;
    private Target target;

    public int targetsCount;
    private float dist;
    

    public void ResetArea()
    {
        targetsCount = 4;
        shooter.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
       /* foreach (var spawnArea in spawnAreas)
        {
            spawnArea.ResetSpawnArea();
        }*/
        dist = shooter.m_ResetParams.GetWithDefault("distance", 5);
        if(selfPlay)
        {
            shooter.transform.position = new Vector3(transform.position.x - dist, transform.position.y + 0.5f, transform.position.z - dist);
            shooter2.transform.position = new Vector3(transform.position.x + dist, transform.position.y + 0.5f, transform.position.z + dist);
        }
    }   
    
    private void Update()
    {        
        if(targetsCount <= 0 && !selfPlay)
        {
            shooter.EndEpisode();
        }

         cumulativeRewardTextRew.text = shooter.GetCumulativeReward().ToString("0.00");
        if(selfPlay) cumulativeRewardTextRew2.text = shooter2.GetCumulativeReward().ToString("0.00");

        if (selfPlay) cumulativeRewardText.text = shooter.Health.ToString("0.0");
        if(selfPlay) cumulativeRewardText2.text = shooter2.Health.ToString("0.0");
        
    }
}
