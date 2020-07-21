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
        if(health <= 0)
        {
            shooter?.AddReward(0.05f); 
            if(GetComponent<Enemy>() != null)
            {
                shooter?.AddReward(0.1f);
                GetComponentInParent<InfiniteSpawner>().ResumeSpawning();
            }
            Destroy(this.transform.gameObject);
            
        }
        
    }

    public void ChangeHealth(int amount)
    {
        health = amount;
    }
}
