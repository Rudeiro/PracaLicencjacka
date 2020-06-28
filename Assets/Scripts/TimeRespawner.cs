using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeRespawner : MonoBehaviour
{
    [SerializeField]
    float TimeToRespawn;
    [SerializeField]
    GameObject respawnPrefab;    
    [SerializeField]
    Vector2 xRange;    
    [SerializeField]
    Vector2 zRange;
    [SerializeField]
    Vector2 restrictedXRange;
    [SerializeField]
    Vector2 restrictedZRange;
    [SerializeField]
    WorldArea worldArea;

    private float respawningTime;
    private GameObject respawnObject;
    
    public void ResetRespawner()
    {
        if(respawnObject != null)
        {
            Destroy(respawnObject);
            Spawn();
        }
    }

    private void Spawn()
    {

        float x = UnityEngine.Random.Range(xRange.x, xRange.y) + transform.position.x;
        float z = UnityEngine.Random.Range(zRange.x, zRange.y) + transform.position.z;
        while(x > restrictedXRange.x && x < restrictedXRange.y)
        {
            x = UnityEngine.Random.Range(xRange.x, xRange.y) + transform.position.x;
        }
        while (z > restrictedZRange.x && z < restrictedZRange.y)
        {
            z = UnityEngine.Random.Range(zRange.x, zRange.y) + transform.position.x;
        }
        respawnObject = Instantiate(respawnPrefab);
        respawnObject.transform.position = new Vector3(x, respawnObject.transform.position.y, z);
        if (respawnObject.GetComponent<Target>() != null)
        {
            float targetSize = worldArea.shooter.m_ResetParams.GetWithDefault("target_size", 1f);
            respawnObject.gameObject.transform.localScale = new Vector3(targetSize, targetSize, targetSize);
        }
        respawnObject.gameObject.transform.parent = transform.parent;
    }

    
    void FixedUpdate()
    {
        if(respawnObject == null)
        {
            if(respawningTime >= TimeToRespawn)
            {
                Spawn();
                respawningTime = 0;
            }
            else
            {
                respawningTime += Time.fixedDeltaTime;
            }
        }
    }
}
