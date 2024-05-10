using UnityEngine;

public class StaticRoomManager : RoomManager
{
  // Singleton pattern
  private void Awake()
  {
    if (instance == null)
    {
      instance = this;
      DontDestroyOnLoad(gameObject);
    }
    else if (instance != this)
    {
      Destroy(gameObject);
    }
  }
  void Start()
  {
    Vector2 playerSize = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>().bounds.size;
    GameObject[] rooms = GameObject.FindGameObjectsWithTag("Room");
    foreach (var room in rooms)
    {
      RoomSetup.SetupRoom(room, playerSize);
    }
    rooms[0].GetComponent<Room>().IsStartRoom = true;
    rooms[0].SetActive(true);
    currentRoom = rooms[0];
  }
}