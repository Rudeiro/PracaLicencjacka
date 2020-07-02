using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    [SerializeField]
    Vector2 xRange;
    [SerializeField]
    Vector2 yRange;
    [SerializeField]
    Vector2 zRange;
    [SerializeField]
    int targetsCount;
    [SerializeField]
    GameObject targetPrefab;

    private float targetSize;
    private List<GameObject> targets = new List<GameObject>();
    
    public List<GameObject> Targets { get { return targets; } }
    public int TargetsCount { get { return targetsCount; } }

    private void Spawn()
    {
        for(int i = 0; i < targetsCount; i++)
        {
            targetSize = GetComponentInParent<Room>().WorldArea.shooter.m_ResetParams.GetWithDefault("target_size", 1.5f);
            float x = UnityEngine.Random.Range((int)xRange.x, (int)xRange.y) + transform.position.x;
            //float y = UnityEngine.Random.Range((int)yRange.x, (int)yRange.y) + transform.position.y;
            float z = UnityEngine.Random.Range((int)zRange.x, (int)zRange.y) + transform.position.z;
            targets.Add(Instantiate(targetPrefab, new Vector3(x, targetSize/2, z), Quaternion.identity));
            targets[i].gameObject.transform.localScale = new Vector3(targetSize, targetSize, targetSize);
            targets[i].gameObject.transform.parent = transform.parent.transform.parent;
        }
    }

    public void ResetSpawnArea()
    {
        for(int i = 0; i < targets.Count; i++)
        {
            if(targets[i] != null)
            {
                Destroy(targets[i].gameObject);
            }
        }
        targets = new List<GameObject>();
        Spawn();
    }
}
