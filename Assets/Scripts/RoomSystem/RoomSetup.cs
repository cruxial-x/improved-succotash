using UnityEngine;

public class RoomSetup
{
  public static void SetupRoom(GameObject room, Vector2 playerSize)
  {
    GameObject triggerObject = new("Trigger");
    triggerObject.transform.position = room.transform.position;
    Room roomComponent = room.GetComponent<Room>();
    Vector2 roomSize = roomComponent.RoomSize;
    Debug.Log("Room bounds size: " + roomSize);
    roomComponent.cam.SetRoomPosition(room.transform.position);

    BoxCollider2D collider = triggerObject.AddComponent<BoxCollider2D>();
    collider.isTrigger = true;
    collider.size = new Vector2(roomSize.x - playerSize.x, roomSize.y - playerSize.y);

    RoomTrigger roomTrigger = triggerObject.AddComponent<RoomTrigger>();
    roomTrigger.room = room;

    room.SetActive(false);
  }
}