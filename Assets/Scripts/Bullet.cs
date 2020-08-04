using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed;
    private Rigidbody rigidbody;

    public int bulletDamage;
    public InfinityAgent bulletOwner;

    [SerializeField]
    float bulletRange = 5f;
    private float bulletLifetime = 0;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rigidbody.MovePosition(transform.position + transform.forward * bulletSpeed * Time.fixedDeltaTime);
        if(bulletLifetime > bulletRange)
        {
            Destroy(transform.gameObject);
        }
        else
        {
            bulletLifetime += Time.fixedDeltaTime;
        }
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("enemy") && bulletOwner != null)
        {
            //Debug.LogError("hit");
            other.gameObject.GetComponent<EnemyAgent>().DealDamage(bulletDamage, bulletOwner);
            Destroy(transform.gameObject);
        }
        if (other.transform.CompareTag("destroyableObstacle"))
        {
            //Debug.LogError("hit");
            other.gameObject.GetComponent<Target>().DealDamage(bulletDamage, bulletOwner);
            Destroy(transform.gameObject);
        }
        if (other.transform.CompareTag("Untagged") || other.transform.CompareTag("obstacle"))
        {
            // bulletOwner.AddReward(-0.05f);
            //bulletOwner.AddReward(-bulletOwner.m_ResetParams.GetWithDefault("miss_penalty", 0.01f));
            Destroy(transform.gameObject);
        }
        if (other.transform.CompareTag("Player")) 
        {
            //Debug.LogError("hit");
            other.gameObject.GetComponent<InfinityAgent>().DealDamage(bulletDamage, bulletOwner);
            Destroy(transform.gameObject);
        }

    }
}
