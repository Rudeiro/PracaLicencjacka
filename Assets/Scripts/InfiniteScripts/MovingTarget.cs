using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTarget : MonoBehaviour
{
    [SerializeField]
    float speed;


    private InfiniteWorldArea area;
    private float timeCounter;
    private float timeToChangeDir;
    void Start()
    {
        area = GetComponentInParent<InfiniteWorldArea>();
        timeToChangeDir = UnityEngine.Random.Range(0.1f, 0.2f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(speed > 0 && transform.position.x < area.transform.position.x + 4.625)
        {
            transform.position = new Vector3(transform.position.x + Time.fixedDeltaTime * speed, transform.position.y, transform.position.z);
        }
        else if(transform.position.x > area.transform.position.x + 0.375 && speed < 0)
        {
            transform.position = new Vector3(transform.position.x + Time.fixedDeltaTime * speed, transform.position.y, transform.position.z);

        }
        if(timeCounter > timeToChangeDir)
        {
            timeCounter = 0;
            speed *= -1;
            timeToChangeDir = UnityEngine.Random.Range(0.5f, 3f);
        }
        else
        {
            timeCounter += Time.fixedDeltaTime;
        }
    }
}
