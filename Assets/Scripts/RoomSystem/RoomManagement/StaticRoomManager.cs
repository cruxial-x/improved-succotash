#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class StaticRoomManager : RoomManager
{
  void Start()
  {
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
  public void PopulateRoomList()
  {
    roomList.Clear();
    GameObject[] roomObjects = GameObject.FindGameObjectsWithTag("Room");

    foreach (GameObject roomObject in roomObjects)
    {
      Room room = roomObject.GetComponent<Room>();
      if (room != null)
      {
        roomList.Add(room);
      }
    }
  }
#if UNITY_EDITOR
  [CustomEditor(typeof(StaticRoomManager))]
  public class StaticRoomManagerEditor : Editor
  {
    public override void OnInspectorGUI()
    {
      DrawDefaultInspector();

      StaticRoomManager myScript = (StaticRoomManager)target;
      if (GUILayout.Button("Populate Room List"))
      {
        myScript.PopulateRoomList();
      }
    }
  }
#endif
}