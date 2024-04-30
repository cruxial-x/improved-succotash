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
    // Get the player's size
    GameObject player = GameObject.FindGameObjectWithTag("Player");
    Vector2 playerSize = player.GetComponent<Collider2D>().bounds.size;
    for (int i = 0; i < rooms.Length; i++)
    {
      // Calculate the position of the room based on its index and the specified distances
      Vector3 position = new Vector3(0, i * 20f, 0);

      // Instantiate the room at the calculated position
      GameObject room = Instantiate(rooms[i], position, Quaternion.identity);

      // Get the camera attached to the room
      Camera roomCamera = room.GetComponentInChildren<Camera>();
      float height = 2f * roomCamera.orthographicSize;
      float width = height * roomCamera.aspect;

      // Create a new GameObject for the trigger collider
      GameObject triggerObject = new GameObject("Trigger");
      triggerObject.transform.position = room.transform.position;

      // Add a BoxCollider2D and set it as a trigger
      BoxCollider2D collider = triggerObject.AddComponent<BoxCollider2D>();
      collider.isTrigger = true;
      // Add an offset to the size of the collider based on the size of the player
      collider.size = new Vector2(width - playerSize.x, height - playerSize.y);

      // Add the RoomTrigger script and set the room
      RoomTrigger roomTrigger = triggerObject.AddComponent<RoomTrigger>();
      roomTrigger.room = room;

      // Deactivate the room
      room.SetActive(false);

      // If this is the first room, set it as the current room
      if (i == 0)
      {
        currentRoom = room;
      }
    }

    // Activate the first room
    currentRoom.SetActive(true);
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
public class RoomTrigger : MonoBehaviour
{
  public GameObject room;

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag("Player"))
    {
      RoomManager.instance.currentRoom.SetActive(false);
      RoomManager.instance.DisableRoomCamera(RoomManager.instance.currentRoom);
      RoomManager.instance.currentRoom = room;
      RoomManager.instance.EnableRoomCamera(room);
    }
  }
}