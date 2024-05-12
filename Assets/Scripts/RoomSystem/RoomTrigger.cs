using UnityEngine;
public class RoomTrigger : MonoBehaviour
{
  public Room room;

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag("Player"))
    {
      if (!room.isVisited)
      {
        room.isVisited = true;
      }
      RoomManager.instance.SwitchRoom(room.gameObject);
    }
  }
}