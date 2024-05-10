using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
  public Vector3 spawnPoint = new Vector3(0, 0, 0);
  public Camera roomCamera { get; private set; }
  public List<Door> doors = new List<Door>();
  public bool IsStartRoom { get; set; }
  public CameraFollow cam;
  public float offset = 3;
  public Vector2 RoomSize
  {
    get
    {
      return cam.GetBoundsSize();
    }
  }
  // Start is called before the first frame update
  void Start()
  {
    GameObject player = GameObject.FindWithTag("Player");
    if (spawnPoint != Vector3.zero && IsStartRoom)
    {
      player.transform.position = spawnPoint;
    }
    roomCamera = GetComponentInChildren<Camera>();
  }
  public Vector3 GetDoorPosition(Door door) => door switch
  {
    Door.TopMiddle => new Vector3(0, RoomSize.y / 2, 0),
    Door.BottomMiddle => new Vector3(0, -RoomSize.y / 2, 0),
    Door.TopLeft => new Vector3(-RoomSize.x / 2 + offset, RoomSize.y / 2, 0),
    Door.BottomLeft => new Vector3(-RoomSize.x / 2 + offset, -RoomSize.y / 2, 0),
    Door.TopRight => new Vector3(RoomSize.x / 2 - offset, RoomSize.y / 2, 0),
    Door.BottomRight => new Vector3(RoomSize.x / 2 - offset, -RoomSize.y / 2, 0),
    Door.LeftTop => new Vector3(-RoomSize.x / 2, RoomSize.y / 2 - offset, 0),
    Door.RightTop => new Vector3(RoomSize.x / 2, RoomSize.y / 2 - offset, 0),
    Door.LeftMiddle => new Vector3(-RoomSize.x / 2, 0, 0),
    Door.RightMiddle => new Vector3(RoomSize.x / 2, 0, 0),
    Door.LeftBottom => new Vector3(-RoomSize.x / 2, -RoomSize.y / 2 + offset, 0),
    Door.RightBottom => new Vector3(RoomSize.x / 2, -RoomSize.y / 2 + offset, 0),
    _ => Vector3.zero
  };
  // Update is called once per frame
  void Update()
  {

  }
  void OnDrawGizmos()
  {
    // Draw doors in the scene view
    Gizmos.color = Color.green;
    foreach (Door door in doors)
    {
      Gizmos.DrawWireSphere(transform.position + GetDoorPosition(door), 0.5f);
    }
  }
}
public enum Door
{
  TopMiddle,
  BottomMiddle,
  TopLeft,
  BottomLeft,
  TopRight,
  BottomRight,
  LeftTop,
  RightTop,
  LeftMiddle,
  RightMiddle,
  LeftBottom,
  RightBottom
}