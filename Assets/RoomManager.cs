using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class RoomManager : MonoBehaviour
{
  public static RoomManager instance;
  public GameObject[] rooms;
  public GameObject currentRoom;
  public Dictionary<Door, List<Door>> validDoorConnections = new Dictionary<Door, List<Door>>
    {
        { Door.TopMiddle, new List<Door> { Door.BottomMiddle } },
        { Door.BottomMiddle, new List<Door> { Door.TopMiddle } },
        { Door.TopLeft, new List<Door> { Door.BottomLeft, Door.TopRight } },
        { Door.BottomLeft, new List<Door> { Door.TopLeft, Door.BottomRight } },
        { Door.TopRight, new List<Door> { Door.BottomRight, Door.TopLeft } },
        { Door.BottomRight, new List<Door> { Door.TopRight, Door.BottomLeft } },
        { Door.LeftTop, new List<Door> { Door.RightTop } },
        { Door.RightTop, new List<Door> { Door.LeftTop } },
        { Door.LeftMiddle, new List<Door> { Door.RightMiddle } },
        { Door.RightMiddle, new List<Door> { Door.LeftMiddle } },
        { Door.LeftBottom, new List<Door> { Door.RightBottom } },
        { Door.RightBottom, new List<Door> { Door.LeftBottom } }
    };

  // Singleton pattern
  private void Awake()
  {
    if (instance == null)
    {
      instance = this;
    }
    else
    {
      Destroy(this);
    }
  }

  // Start is called before the first frame update
  void Start()
  {
    InstantiateAllRooms();
  }

  // Update is called once per frame
  void Update()
  {

  }
  bool HorizontalConnection(Door door1, Door door2)
  {
    return (door1 == Door.LeftMiddle && door2 == Door.RightMiddle) ||
    (door1 == Door.RightMiddle && door2 == Door.LeftMiddle) ||
    (door1 == Door.LeftTop && door2 == Door.RightTop) ||
    (door1 == Door.RightTop && door2 == Door.LeftTop) ||
    (door1 == Door.LeftBottom && door2 == Door.RightBottom) ||
    (door1 == Door.RightBottom && door2 == Door.LeftBottom);
  }
  bool VerticalConnection(Door door1, Door door2)
  {
    return (door1 == Door.TopMiddle && door2 == Door.BottomMiddle) ||
    (door1 == Door.BottomMiddle && door2 == Door.TopMiddle) ||
    (door1 == Door.TopLeft && door2 == Door.BottomLeft) ||
    (door1 == Door.BottomLeft && door2 == Door.TopLeft) ||
    (door1 == Door.TopRight && door2 == Door.BottomRight) ||
    (door1 == Door.BottomRight && door2 == Door.TopRight);
  }
  void InstantiateAllRooms()
  {
    Vector2 roomSize = Vector2.zero;
    GameObject player = GameObject.FindGameObjectWithTag("Player");
    Vector2 playerSize = player.GetComponent<Collider2D>().bounds.size;

    Dictionary<GameObject, Vector3> roomPositions = new Dictionary<GameObject, Vector3>();
    List<GameObject> roomList = new List<GameObject>();
    // Instantiate the first room at the origin
    if (rooms.Length > 0)
    {
      GameObject firstRoom = Instantiate(rooms[0], Vector3.zero, Quaternion.identity);
      firstRoom.GetComponent<Room>().IsStartRoom = true;
      roomPositions.Add(firstRoom, Vector3.zero);
      roomList.Add(firstRoom);
      SetupRoom(firstRoom, playerSize);
    }
    for (int i = 1; i < rooms.Length; i++)
    {
      GameObject roomPrefab = rooms[i];
      List<Door> roomDoors = roomPrefab.GetComponent<Room>().doors;
      List<Vector3> possiblePositions = new List<Vector3>();

      foreach (Door door in roomDoors)
      {
        foreach (var entry in roomPositions)
        {
          Room room = roomPrefab.GetComponent<Room>();
          roomSize = room.RoomSize;
          GameObject otherRoomGo = entry.Key;
          Room otherRoom = otherRoomGo.GetComponent<Room>();
          Vector2 otherRoomSize = otherRoom.RoomSize;
          Vector3 otherPosition = entry.Value;
          List<Door> otherRoomDoors = otherRoom.doors;

          foreach (Door otherDoor in otherRoomDoors)
          {
            if (validDoorConnections.ContainsKey(door) && validDoorConnections[door].Contains(otherDoor))
            {
              Vector3 newPosition = Vector3.zero;
              if (VerticalConnection(door, otherDoor))
              {
                float yOffset = (door == Door.TopMiddle || door == Door.TopLeft || door == Door.TopRight) ? -(roomSize.y + otherRoomSize.y) / 2 : (roomSize.y + otherRoomSize.y) / 2;
                newPosition = new Vector3(otherPosition.x, otherPosition.y + yOffset, 0);
              }
              else if (HorizontalConnection(door, otherDoor))
              {
                float xOffset = (door == Door.LeftMiddle || door == Door.LeftTop || door == Door.LeftBottom) ? (roomSize.x + otherRoomSize.x) / 2 : -(roomSize.x + otherRoomSize.x) / 2;
                newPosition = new Vector3(otherPosition.x + xOffset, otherPosition.y, 0);
              }
              possiblePositions.Add(newPosition);
            }
          }
        }
      }

      Vector3? position = ResolvePositionConflicts(possiblePositions, roomPositions, roomSize);
      if (position != null)
      {
        // Instantiate the room at the chosen position
        GameObject room = Instantiate(roomPrefab, position.Value, Quaternion.identity);
        room.GetComponent<Room>().cam.SetRoomPosition(position.Value);
        roomPositions.Add(room, position.Value);
        roomList.Add(room);

        // Setup the room
        SetupRoom(room, playerSize);
      }
    }

    // Activate the first room
    if (roomList.Count > 0)
    {
      roomList[0].SetActive(true);
      currentRoom = roomList[0];
    }
  }

  Vector3? ResolvePositionConflicts(List<Vector3> positions, Dictionary<GameObject, Vector3> roomPositions, Vector2 newRoomSize)
  {
    foreach (var position in positions)
    {
      bool overlaps = false;
      foreach (var kvp in roomPositions)
      {
        GameObject existingRoom = kvp.Key;
        Vector3 existingPosition = kvp.Value;
        Vector2 existingRoomSize = existingRoom.GetComponent<Room>().RoomSize;  // Assuming Room has a RoomSize property

        // Calculate if rooms overlap using actual room dimensions
        if (Mathf.Abs(position.x - existingPosition.x) < (newRoomSize.x / 2 + existingRoomSize.x / 2) &&
            Mathf.Abs(position.y - existingPosition.y) < (newRoomSize.y / 2 + existingRoomSize.y / 2))
        {
          overlaps = true;
          Debug.Log($"Overlap detected between new position {position} and existing position {existingPosition} with new room size {newRoomSize} and existing room size {existingRoomSize}");
          break;
        }
      }
      if (!overlaps)
      {
        Debug.Log($"No overlap found, position {position} is valid");
        return position;
      }
    }
    Debug.Log("No valid positions found due to overlaps");
    return null;
  }

  void SetupRoom(GameObject room, Vector2 playerSize)
  {
    GameObject triggerObject = new("Trigger");
    triggerObject.transform.position = room.transform.position;
    Vector2 roomSize = room.GetComponent<Room>().RoomSize;
    Debug.Log("Room bounds size: " + roomSize);

    BoxCollider2D collider = triggerObject.AddComponent<BoxCollider2D>();
    collider.isTrigger = true;
    collider.size = new Vector2(roomSize.x - playerSize.x, roomSize.y - playerSize.y);

    RoomTrigger roomTrigger = triggerObject.AddComponent<RoomTrigger>();
    roomTrigger.room = room;

    room.SetActive(false);
  }
  public void EnableRoomCamera(GameObject room)
  {
    if (!room.activeInHierarchy)
    {
      room.SetActive(true);
    }

    Camera roomCamera = room.GetComponentInChildren<Camera>();
    if (roomCamera != null)
    {
      roomCamera.enabled = true;
    }
  }

  public void DisableRoomCamera(GameObject room)
  {
    Camera roomCamera = room.GetComponentInChildren<Camera>();
    if (roomCamera != null)
    {
      roomCamera.enabled = false;
    }
  }
}