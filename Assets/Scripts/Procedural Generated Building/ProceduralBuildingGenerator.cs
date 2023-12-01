using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ProceduralBuildingGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject wallPrefab;
    
    [SerializeField]
    private GameObject roofPrefab;

    [SerializeField]
    private bool includeRoof = false;

    [SerializeField]
    private int width = 3;

    [SerializeField]
    private int height = 3;

    [SerializeField]
    private float cellUnitSize = 1;

    [SerializeField]
    private int numberOfFloors = 1;

    [SerializeField]
    private Floor[] floors;

    void Awake()
    {
        Generate(); // Create data structure
        Render(); // Generate prefabs
    }

    void Generate()
    {
        floors = new Floor[numberOfFloors];

        int floorCount = 0;
        foreach(Floor floor in floors)
        {
            Room[,] rooms = new Room[width, height];

            for(int i = 0; i < width; i++)
            {
                for(int j = 0; j < height; j++)
                {
                    rooms[i, j] = new Room(new Vector2(i * cellUnitSize, j * cellUnitSize), includeRoof ? (floorCount == floors.Length - 1) : false);
                }
            }
            floors[floorCount] = new Floor(floorCount++, rooms);
        }
    }

    void Render()
    {
        foreach(Floor floor in floors)
        {
            for(int i = 0; i < width; i++)
            {
                for(int j = 0; j < height; j++)
                {
                    Room room = floor.rooms[i, j];
                    var wall1 = Instantiate(wallPrefab, new Vector3(room.RoomPosition.x, floor.FloorNumber, room.RoomPosition.y), Quaternion.Euler(0, 0, 0));
                    wall1.transform.parent = transform;
                    var wall2 = Instantiate(wallPrefab, new Vector3(room.RoomPosition.x, floor.FloorNumber, room.RoomPosition.y), Quaternion.Euler(0, 90, 0));
                    wall2.transform.parent = transform;
                    var wall3 = Instantiate(wallPrefab, new Vector3(room.RoomPosition.x, floor.FloorNumber, room.RoomPosition.y), Quaternion.Euler(0, 180, 0));
                    wall3.transform.parent = transform;
                    var wall4 = Instantiate(wallPrefab, new Vector3(room.RoomPosition.x, floor.FloorNumber, room.RoomPosition.y), Quaternion.Euler(0, -90, 0));
                    wall4.transform.parent = transform;

                    if(room.HasRoof)
                    {
                        var roof = Instantiate(roofPrefab, new Vector3(room.RoomPosition.x, floor.FloorNumber, room.RoomPosition.y), Quaternion.identity);
                        roof.transform.parent = transform;
                    }
                }
            }
        }
    }
}
