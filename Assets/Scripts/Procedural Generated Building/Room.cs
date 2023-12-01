using UnityEngine;

public class Room
{
    public Wall[] Walls;
    private Vector2 position;
    public bool HasRoof {get; private set; }

    public Room(Vector2 position, bool hasRoof = false)
    {
        this.position = position;
        this.HasRoof = hasRoof;
    }

    public Vector2 RoomPosition
    {
        get
        {
            return this.position;
        }
    }
}
