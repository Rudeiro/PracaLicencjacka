using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Obstacle : MonoBehaviour
{
    public enum Type
    {
        FirstAid,
        Asteroid
    }

    [SerializeField]
    Type type;
    public float speed;  
    
    
    void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - Time.fixedDeltaTime * speed);
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && type == Type.Asteroid)
        {
            //other.gameObject.GetComponent<InfinityAgent>().AddReward(-1f);
            other.gameObject.GetComponent<InfinityAgent>().DealDamage(50, null);
            //other.gameObject.GetComponent<InfinityAgent>().EndEpisode();
            Destroy(transform.gameObject);
        }

        if (other.gameObject.CompareTag("obstacleDestroyer"))
        {
            Destroy(transform.gameObject);
        }

        if (other.gameObject.CompareTag("obstacle") || other.gameObject.CompareTag("destroyableObstacle"))
        {
            GetComponentInParent<InfiniteSpawner>().Respawn(transform.gameObject);
            //Debug.LogError("xD");
        }
    }

    
}
