using UnityEngine;

public class StaticRoomManager : RoomManager
{
  void Start()
  {
    roomList.Clear();
    Vector2 playerSize = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>().bounds.size;
    GameObject[] roomObjects = GameObject.FindGameObjectsWithTag("Room");

    for (int i = 0; i < roomObjects.Length; i++)
    {
      Room room = roomObjects[i].GetComponent<Room>();
      room.Setup(playerSize);
      roomList.Add(room);
    }

    roomObjects[0].GetComponent<Room>().IsStartRoom = true;
    roomObjects[0].SetActive(true);
    currentRoom = roomObjects[0];
  }
}