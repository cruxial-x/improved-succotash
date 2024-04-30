using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
  public GameObject[] rooms;
  // Start is called before the first frame update
  void Start()
  {
    InstantiateFirstRoom();
  }

  // Update is called once per frame
  void Update()
  {

  }
  void InstantiateRoom(int roomIndex)
  {
    Instantiate(rooms[roomIndex], transform.position, Quaternion.identity);
  }
  void InstantiateRandomRoom()
  {
    int randomRoomIndex = Random.Range(0, rooms.Length);
    InstantiateRoom(randomRoomIndex);
  }
  void InstantiateFirstRoom()
  {
    InstantiateRoom(0);
  }
}
