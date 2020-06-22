using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaner : MonoBehaviour
{
    [SerializeField]
    float scanningSpeed;

    private int scanningDir = -1;
    

    // Update is called once per frame
    void FixedUpdate()
    {
        if(transform.eulerAngles.x < 330 && transform.eulerAngles.x > 2)
        {
            scanningDir = 1;
        }
        if (transform.eulerAngles.x > 1 && transform.eulerAngles.x < 328)
        {
            scanningDir = -1;
        }
        transform.Rotate(scanningDir * scanningSpeed * Time.fixedDeltaTime, 0, 0);
    }
}
