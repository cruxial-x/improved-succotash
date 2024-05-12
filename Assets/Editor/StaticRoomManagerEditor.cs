#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StaticRoomManager))]
public class StaticRoomManagerEditor : Editor
{
  public override void OnInspectorGUI()
  {
    DrawDefaultInspector();

    StaticRoomManager myScript = (StaticRoomManager)target;
    if (GUILayout.Button("Populate Room List"))
    {
      PopulateRoomList(myScript);
    }
  }

  private void PopulateRoomList(StaticRoomManager manager)
  {
    manager.roomList.Clear();
    GameObject[] roomObjects = GameObject.FindGameObjectsWithTag("Room");

    foreach (GameObject roomObject in roomObjects)
    {
      Room room = roomObject.GetComponent<Room>();
      if (room != null)
      {
        manager.roomList.Add(room);
      }
    }
  }
}
#endif