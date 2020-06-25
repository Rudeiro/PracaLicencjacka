using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject itemPrefab;

    public GameObject Item;

    public void ResetItemSpawner()
    {
        if (Item != null) Destroy(Item);
        Item = Instantiate(itemPrefab, transform.position, Quaternion.identity);
        Item.transform.parent = transform; 
    }

    

}
