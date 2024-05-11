using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class CameraFollow : MonoBehaviour
{
  [SerializeField] Vector3 offset;
  public Vector3 minCameraPos = new(0, 0, 0);
  public Vector3 maxCameraPos = new(0, 0, 0);
  [HideInInspector] public Vector3 minEdgePos;
  [HideInInspector] public Vector3 maxEdgePos;
  private Camera cam;
  private Transform player;

  void Awake()
  {
    player = GameObject.FindWithTag("Player").transform;
    cam = GetComponent<Camera>();
    GetCameraBounds();
  }
  public void SetRoomPosition(Vector3 roomPosition)
  {
    // Adjust the camera's min and max positions by the room's position
    minCameraPos += roomPosition;
    maxCameraPos += roomPosition;

    // Update the camera's edge positions
    GetCameraBounds();
  }
  void LateUpdate()
  {
    float posX = Mathf.Clamp(player.position.x + offset.x, minCameraPos.x, maxCameraPos.x);
    float posY = Mathf.Clamp(player.position.y + offset.y, minCameraPos.y, maxCameraPos.y);

    transform.position = new Vector3(posX, posY, transform.position.z + offset.z);
  }
  void GetCameraBounds()
  {
    // Get the camera's size
    float height = cam.orthographicSize * 2;
    float width = height * cam.aspect;

    // Calculate the bounds for the edges of the camera
    minEdgePos = minCameraPos - new Vector3(width / 2, height / 2, 0);
    maxEdgePos = maxCameraPos + new Vector3(width / 2, height / 2, 0);
  }
  public Vector2 GetBoundsSize()
  {
    return new Vector2(maxEdgePos.x - minEdgePos.x, maxEdgePos.y - minEdgePos.y);
  }
  void OnDrawGizmos()
  {
    if (cam == null) cam = GetComponent<Camera>();
    GetCameraBounds();
    // Draw a red line box representing the camera bounds
    Gizmos.color = Color.red;
    Gizmos.DrawLine(new Vector3(minEdgePos.x, minEdgePos.y, 0), new Vector3(maxEdgePos.x, minEdgePos.y, 0));
    Gizmos.DrawLine(new Vector3(maxEdgePos.x, minEdgePos.y, 0), new Vector3(maxEdgePos.x, maxEdgePos.y, 0));
    Gizmos.DrawLine(new Vector3(maxEdgePos.x, maxEdgePos.y, 0), new Vector3(minEdgePos.x, maxEdgePos.y, 0));
    Gizmos.DrawLine(new Vector3(minEdgePos.x, maxEdgePos.y, 0), new Vector3(minEdgePos.x, minEdgePos.y, 0));
  }
}