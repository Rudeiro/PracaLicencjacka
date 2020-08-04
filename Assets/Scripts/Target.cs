using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField]
    int health;

    public void DealDamage(int amount, InfinityAgent shooter)
    {
        health -= amount;
        shooter?.AddReward(0.005f);
        if (health <= 0)
        {
            shooter?.AddReward(0.001f); 
           /* if(GetComponent<EnemyAgent>() != null)
            {
                shooter?.AddReward(0.1f);
                GetComponentInParent<InfiniteSpawner>().ResumeSpawning();
            }*/
            //shooter.EndEpisode();
            Destroy(this.transform.gameObject);
            
        }
        
    }

    public void ChangeHealth(int amount)
    {
        health = amount;
    }
}
