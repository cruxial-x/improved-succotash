using UnityEngine;

public class CameraManager
{
  public static void EnableRoomCamera(GameObject room)
  {
    if (!room.activeInHierarchy)
    {
      room.SetActive(true);
    }

    Camera roomCamera = room.GetComponentInChildren<Camera>();
    if (roomCamera != null)
    {
      roomCamera.enabled = true;
    }
  }

  public static void DisableRoomCamera(GameObject room)
  {
    Camera roomCamera = room.GetComponentInChildren<Camera>();
    if (roomCamera != null)
    {
      roomCamera.enabled = false;
    }
  }
}