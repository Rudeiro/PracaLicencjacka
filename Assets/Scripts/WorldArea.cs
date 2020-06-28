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
    List<SpawnArea> spawnAreas;
    [SerializeField]
    bool selfPlay;
    [SerializeField]
    List<Room> rooms;
    [SerializeField]
    List<TimeRespawner> timeRespawners;
    [SerializeField]
    int TargetsToReset;
    [SerializeField]
    int DamageToAgent;
    [SerializeField]
    float DealDamageRate;


    public int targetsCount;
    private float dist;
    private float timeToDamage;

    public void ResetArea()
    {

        timeToDamage = 0;
        targetsCount = 0;
        // targetsCount = 0;
        /* for(int i = 0; i < rooms.Count; i++)
         {
             if(i < shooter.m_ResetParams.GetWithDefault("rooms_unlocked", 9))
             {
                 rooms[i].gameObject.SetActive(true);
                 rooms[i].ResetRoom();
                 targetsCount += rooms[i].TargetsCount;
             }
         }*/
        foreach (var timeRespawner in timeRespawners)
        {
            timeRespawner.ResetRespawner();
        }
        shooter.transform.position = new Vector3(transform.position.x , transform.position.y + 0.5f, transform.position.z );
       /* foreach (var spawnArea in spawnAreas)
        {
            spawnArea.ResetSpawnArea();
        }*/
        //dist = shooter.m_ResetParams.GetWithDefault("distance", 5);
        /*if(selfPlay)
        {
            shooter.transform.position = new Vector3(transform.position.x - dist, transform.position.y + 0.5f, transform.position.z - dist);
            shooter2.transform.position = new Vector3(transform.position.x + dist, transform.position.y + 0.5f, transform.position.z + dist);
        }*/
    }

    private void FixedUpdate()
    {
        /*if(targetsCount <= 0 && !selfPlay)
        {
            shooter.EndEpisode();
        }*/
        if (targetsCount >= TargetsToReset)
        {
            targetsCount = 0;
            shooter.EndEpisode();
        }
        cumulativeRewardTextRew.text = shooter.GetCumulativeReward().ToString("0.00");
        if (selfPlay) cumulativeRewardTextRew2.text = shooter2.GetCumulativeReward().ToString("0.00");

        if (selfPlay) cumulativeRewardText.text = shooter.Health.ToString("0.0");
        if (selfPlay) cumulativeRewardText2.text = shooter2.Health.ToString("0.0");
        if (shooter.m_ResetParams.GetWithDefault("receive_damage", 1) == 1)
        {
            if (timeToDamage > DealDamageRate)
            {
                shooter.DealDamage(10, null);
                timeToDamage = 0;
            }
            else
            {
                timeToDamage += Time.fixedDeltaTime;

            }
        }
    }
}
