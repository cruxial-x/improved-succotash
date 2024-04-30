using UnityEngine;
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