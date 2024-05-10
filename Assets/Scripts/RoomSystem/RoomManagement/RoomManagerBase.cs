using UnityEngine;

public abstract class RoomManager : MonoBehaviour
{
  public static RoomManager instance;
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
}