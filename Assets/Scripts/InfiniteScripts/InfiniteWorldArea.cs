using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfiniteWorldArea : MonoBehaviour
{
    [SerializeField]
    InfinityAgent agent;    
    [SerializeField]
    InfiniteSpawner spawner;
    [SerializeField]
    public TextMeshPro rewardText;
    [SerializeField]
    public TextMeshPro distanceText;
    [SerializeField]
    public TextMeshPro recordText;
    [SerializeField]
    float lvlDistChange;

    float lvlDistCounter;
    private float record = 0;

    public InfinityAgent Agent { get { return agent; } }
    public InfiniteSpawner Spawner { get { return spawner; } }

    public void ResetArea()
    {
        agent.transform.position = new Vector3(transform.position.x + 2.5f, transform.position.y + 0.5f, transform.position.z -4.5f);
        //GameObject target = Instantiate(movingTargetPrefab, new Vector3(transform.position.x + 2.5f, transform.position.y + 0.5f, transform.position.z + 6f), Quaternion.identity);
       // target.transform.parent = transform;
        spawner.ResetSpawner();
        lvlDistChange = 100f;
        lvlDistCounter = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rewardText.text = "Reward: " + agent.GetCumulativeReward().ToString("0.00");
        if(agent.DistanceTraveled > record)
        {
            record = agent.DistanceTraveled;
        }
        if(agent.DistanceTraveled < 1000)
        { 
            distanceText.text = "Distance: " + agent.DistanceTraveled.ToString("0.00") + "m";
            recordText.text = "Record: " + record.ToString("0.00") + "m";
        }
        else
        {
            distanceText.text = "Distance: " + (agent.DistanceTraveled/1000).ToString("0.00") + "km";
            recordText.text = "Record: " + (record / 1000).ToString("0.00") + "km";
        }
        if(lvlDistCounter >= lvlDistChange)
        {
            agent.speed *= 1.2f;
            spawner.ChangeSpawnRateAndSpeed(1.2f);
            lvlDistCounter = 0;
            lvlDistChange *= 1.2f;
            spawner.ChangeEnemyParameters(1.2f, 1.2f, 1.2f);
        }
        else
        {
            if (spawner.StopSpawning == false)
            {
                lvlDistCounter += agent.speed * Time.fixedDeltaTime;
            }
        }
    }
}
