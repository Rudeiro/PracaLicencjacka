using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField]
    int health;

    public void DealDamage(int amount, ShooterAgent shooter)
    {
        health -= amount;
        shooter.AddReward(8f);
        if(health <= 0)
        {
            shooter.AddReward(15f);

            //shooter.EndEpisode();
            GetComponentInParent<WorldArea>().targetsCount++;
            //Debug.LogError("end");
            Destroy(this.transform.gameObject);
        }
        
    }
}
