using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    float speed;
    [SerializeField]
    Weapon weapon;
    [SerializeField]
    float flyPosition;

    private InfiniteWorldArea area;
    public Weapon Weapon { get { return weapon; } }

    void Start()
    {
        area = GetComponentInParent<InfiniteWorldArea>();
    }

    
    void FixedUpdate()
    {
        if (Mathf.Abs(area.Agent.transform.position.x - transform.position.x) > 0.1)
        {
            if (area.Agent.transform.position.x > transform.position.x)
            {
                transform.position = new Vector3(transform.position.x + Time.fixedDeltaTime * speed, transform.position.y, transform.position.z);
            }
            else
            {
                transform.position = new Vector3(transform.position.x - Time.fixedDeltaTime * speed, transform.position.y, transform.position.z);
            }
        }
        if(transform.position.z > flyPosition)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - Time.fixedDeltaTime*speed/2);
        }
        weapon.Shoot();
    }
}
