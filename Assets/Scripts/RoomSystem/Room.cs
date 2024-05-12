using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

public class Room : MonoBehaviour
{
  public Vector3 spawnPoint = new Vector3(0, 0, 0);
  public List<Door> doors = new List<Door>();
  public bool isVisited = false;
  private Transform player;
  private float doorOffset = 3;
  public Vector2 roomSize;
  [HideInInspector] public Vector3 minEdgePos;
  [HideInInspector] public Vector3 maxEdgePos;
  private TilemapCollider2D[] tilemapColliders;
  private Camera cam;
  private RoomMap map;

  public Vector2 RoomSize
  {
    get
    {
      if (roomSize.Equals(Vector2.zero))
      {
        InitializeRoom();
      }
      return roomSize;
    }
  }
  public void Setup(Vector2 playerSize, Camera overlayCamera = null)
  {
    if (overlayCamera != null)
    {
      var cameraData = cam.GetUniversalAdditionalCameraData();
      cameraData.cameraStack.Add(overlayCamera);
      map = new RoomMap(this);
    }
    GameObject triggerObject = new("Trigger");
    triggerObject.transform.position = this.transform.position;
    Dev.Log("Room bounds size: " + roomSize);

    BoxCollider2D collider = triggerObject.AddComponent<BoxCollider2D>();
    collider.isTrigger = true;
    collider.size = new Vector2(roomSize.x - playerSize.x, roomSize.y - playerSize.y);

    RoomTrigger roomTrigger = triggerObject.AddComponent<RoomTrigger>();
    roomTrigger.room = this;

    Disable();
  }
  public void SetActive(bool active)
  {
    this.gameObject.SetActive(active);
  }

  void Awake()
  {
    player = GameObject.FindWithTag("Player").transform;
    cam = GetComponentInChildren<Camera>();
    cam.cullingMask = ~LayerMask.GetMask("Hidden") & ~LayerMask.GetMask("Mapped");
    tilemapColliders = GetComponentsInChildren<TilemapCollider2D>();
    InitializeRoom();
  }
  public void Disable()
  {
    map?.Hide();
    if (tilemapColliders != null)
      foreach (TilemapCollider2D tilemapCollider in tilemapColliders)
      {
        tilemapCollider.enabled = false;
      }

    cam.gameObject.SetActive(false);
  }

  public void Enable()
  {
    map?.Show();
    if (tilemapColliders != null)
      foreach (TilemapCollider2D tilemapCollider in tilemapColliders)
      {
        tilemapCollider.enabled = true;
      }

    cam.gameObject.SetActive(true);
  }

  void InitializeRoom()
  {
    if (!roomSize.Equals(Vector2.zero))
    {
      minEdgePos = transform.position + new Vector3(-roomSize.x / 2, -roomSize.y / 2, 0);
      maxEdgePos = transform.position + new Vector3(roomSize.x / 2, roomSize.y / 2, 0);
    }
    else
    {
      if (cam == null) cam = GetComponentInChildren<Camera>();
      // Get the camera's size
      float height = cam.orthographicSize * 2;
      float width = height * cam.aspect;

      // Calculate the bounds for the edges of the camera
      minEdgePos = transform.position - new Vector3(width / 2, height / 2, 0);
      maxEdgePos = transform.position + new Vector3(width / 2, height / 2, 0);
      roomSize = new Vector2(Mathf.Abs(minEdgePos.x - maxEdgePos.x), Mathf.Abs(minEdgePos.y - maxEdgePos.y));
    }
  }

  void LateUpdate()
  {
    float halfCamWidth = cam.orthographicSize * cam.aspect;
    float halfCamHeight = cam.orthographicSize;

    float posX = Mathf.Clamp(player.position.x, minEdgePos.x + halfCamWidth, maxEdgePos.x - halfCamWidth);
    float posY = Mathf.Clamp(player.position.y, minEdgePos.y + halfCamHeight, maxEdgePos.y - halfCamHeight);

    cam.transform.position = new Vector3(posX, posY, cam.transform.position.z);
  }

  public Vector3 GetDoorPosition(Door door) => door switch
  {
    Door.TopMiddle => new Vector3(0, RoomSize.y / 2, 0),
    Door.BottomMiddle => new Vector3(0, -RoomSize.y / 2, 0),
    Door.TopLeft => new Vector3(-RoomSize.x / 2 + doorOffset, RoomSize.y / 2, 0),
    Door.BottomLeft => new Vector3(-RoomSize.x / 2 + doorOffset, -RoomSize.y / 2, 0),
    Door.TopRight => new Vector3(RoomSize.x / 2 - doorOffset, RoomSize.y / 2, 0),
    Door.BottomRight => new Vector3(RoomSize.x / 2 - doorOffset, -RoomSize.y / 2, 0),
    Door.LeftTop => new Vector3(-RoomSize.x / 2, RoomSize.y / 2 - doorOffset, 0),
    Door.RightTop => new Vector3(RoomSize.x / 2, RoomSize.y / 2 - doorOffset, 0),
    Door.LeftMiddle => new Vector3(-RoomSize.x / 2, 0, 0),
    Door.RightMiddle => new Vector3(RoomSize.x / 2, 0, 0),
    Door.LeftBottom => new Vector3(-RoomSize.x / 2, -RoomSize.y / 2 + doorOffset, 0),
    Door.RightBottom => new Vector3(RoomSize.x / 2, -RoomSize.y / 2 + doorOffset, 0),
    _ => Vector3.zero
  };

  void OnDrawGizmos()
  {
    // Draw doors in the scene view
    Gizmos.color = Color.green;
    foreach (Door door in doors)
    {
      Gizmos.DrawWireSphere(transform.position + GetDoorPosition(door), 0.5f);
    }
    if (cam == null) cam = GetComponent<Camera>();

    InitializeRoom();
    // Draw room bounds
    Gizmos.color = Color.cyan;
    Gizmos.DrawLine(minEdgePos, new Vector3(minEdgePos.x, maxEdgePos.y, minEdgePos.z));
    Gizmos.DrawLine(new Vector3(minEdgePos.x, maxEdgePos.y, minEdgePos.z), maxEdgePos);
    Gizmos.DrawLine(maxEdgePos, new Vector3(maxEdgePos.x, minEdgePos.y, maxEdgePos.z));
    Gizmos.DrawLine(new Vector3(maxEdgePos.x, minEdgePos.y, maxEdgePos.z), minEdgePos);
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