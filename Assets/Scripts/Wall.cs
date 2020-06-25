using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private enum WallDirection
    {
        North,
        South,
        West,
        East
    }

    [SerializeField]
    GameObject wallSegmentPrefab;
    [SerializeField]
    GameObject doorPrefab;
    [SerializeField]
    int roomSize;
    [SerializeField]
    WallDirection wallDirection;
    [SerializeField]
    bool hasDoor;
    [SerializeField]
    int segmentSize;

    private List<GameObject> wallSegments = new List<GameObject>();
    private GameObject segment;

    void Start()
    {
        int startPosX = 0;
        int startPosZ = 0;
        int changeX = 0;
        int changeZ = 0;
        int angle = 0 ;


        switch (wallDirection)
        {
            case WallDirection.North:
                angle = 180;
                startPosZ = roomSize * segmentSize - segmentSize;
                changeZ = 1;
                break;
            case WallDirection.South:
                startPosZ = roomSize * segmentSize - segmentSize;
                changeZ = 1;
                break;
            case WallDirection.West:
                angle = 90;
                startPosX = roomSize * segmentSize - segmentSize;
                changeX = 1;
                break;
            case WallDirection.East:
                angle = -90;
                startPosX = roomSize * segmentSize - segmentSize;
                changeX = 1;
                break;
            default:
                break;
        }
        int doorPlacement = UnityEngine.Random.Range(0, roomSize);
        for(int i = 0; i < roomSize; i++)
        {
            if(i == doorPlacement && hasDoor)
            {
                segment = Instantiate(doorPrefab, new Vector3(transform.position.x + startPosX - i * changeX * segmentSize * 2,
                                                              transform.position.y,
                                                              transform.position.z + startPosZ - i * changeZ * segmentSize * 2), Quaternion.identity);
                segment.transform.parent = transform;
                segment.transform.Rotate(0, angle, 0);
            }
            else
            {
                segment = Instantiate(wallSegmentPrefab, new Vector3(transform.position.x + startPosX - i * segmentSize * changeX * 2,
                                                              transform.position.y,
                                                              transform.position.z + startPosZ - i * segmentSize * changeZ * 2), Quaternion.identity);
                segment.transform.parent = transform;
                segment.transform.Rotate(0, angle, 0);
            }
        }
    }

   
}
