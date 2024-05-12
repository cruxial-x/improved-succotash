using UnityEngine;
using System.Collections.Generic;

public abstract class RoomManager : MonoBehaviour
{
  public static RoomManager instance;
  public List<Room> roomList = new();
  public GameObject currentRoom;

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
  public void SwitchRoom(GameObject newRoom)
  {
    if (currentRoom != null)
    {
      currentRoom.SetActive(false);
      CameraManager.DisableRoomCamera(currentRoom);
    }

    currentRoom = newRoom;
    currentRoom.SetActive(true);
    CameraManager.EnableRoomCamera(currentRoom);
  }
}