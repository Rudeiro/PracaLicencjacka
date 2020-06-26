using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType
    {
        pistol,
        firstAid
    }

    [SerializeField]
    List<GameObject> itemPrefabs;
    [SerializeField]
    ItemType itemType;

    private GameObject item;    
    
    public ItemType Type { get { return itemType; } }

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

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("blueAgent"))
        {
            //Debug.LogError("entered");
            other.GetComponent<ShooterAgent>().EnablePickUp(transform.gameObject, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("blueAgent"))
        {
            other.GetComponent<ShooterAgent>().EnablePickUp(null, false);
        }
    }
}
