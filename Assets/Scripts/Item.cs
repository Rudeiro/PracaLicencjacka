using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType
    {
        pistol
    }

    [SerializeField]
    List<GameObject> itemPrefabs;

    private GameObject item;    
    
    public void ResetItem()
    {
        if(item != null)
        {
            Destroy(item.transform);
        }
        SpawnItem();
    }
    
    private void SpawnItem()
    {
        int p = UnityEngine.Random.Range(0, itemPrefabs.Count);
        item = Instantiate(itemPrefabs[p]);
        item.transform.parent = transform;
    }
}
