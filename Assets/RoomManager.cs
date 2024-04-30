using UnityEngine;

public class RoomManager : MonoBehaviour
{
  public static RoomManager instance;
  public GameObject[] rooms;
  public GameObject currentRoom;

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

  void InstantiateAllRooms()
  {
    for (int i = 0; i < rooms.Length; i++)
    {
      // Calculate the position of the room based on its index and the specified distances
      Vector3 position = new Vector3(i * 20f, 0, 0);

      // Instantiate the room at the calculated position
      GameObject room = Instantiate(rooms[i], position, Quaternion.identity);

      // Deactivate the room
      room.SetActive(false);

      // If this is the first room, set it as the current room
      if (i == 0)
      {
        currentRoom = room;
      }

      // Add a trigger collider to each room
      room.AddComponent<BoxCollider2D>().isTrigger = true;
    }

    // Activate the first room
    currentRoom.SetActive(true);
  }

  // Called when the player enters a trigger collider
  private void OnTriggerEnter2D(Collider2D other)
  {
    // Check if the collider belongs to a room
    int roomIndex = System.Array.IndexOf(rooms, other.gameObject);
    if (roomIndex != -1)
    {
      // Deactivate the current room and activate the new room
      currentRoom.SetActive(false);
      currentRoom = rooms[roomIndex];
      currentRoom.SetActive(true);
    }
  }
}