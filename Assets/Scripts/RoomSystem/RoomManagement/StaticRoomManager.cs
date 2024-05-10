using UnityEngine;

public class StaticRoomManager : RoomManager
{
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
  void Update()
  {
    Dev.Log("Test ");
  }
}