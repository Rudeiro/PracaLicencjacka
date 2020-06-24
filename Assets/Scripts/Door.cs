﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("blueAgent"))
        {
            other.GetComponent<ShooterAgent>().AddReward(0.75f);
            Destroy(transform.parent.transform.gameObject);
        }
    }
}
