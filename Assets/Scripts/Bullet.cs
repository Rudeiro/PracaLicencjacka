using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed;
    private Rigidbody rigidbody;

    public int bulletDamage;
    public ShooterAgent bulletOwner;

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
        if (other.transform.CompareTag("enemy"))
        {
            other.gameObject.GetComponent<Target>().DealDamage(bulletDamage, bulletOwner);
            Destroy(transform.gameObject);
        }
        if (other.transform.CompareTag("Untagged"))
        {
            bulletOwner.AddReward(-0.005f);
            Destroy(transform.gameObject);
        }

    }
}
