using UnityEngine;

public class Dev : MonoBehaviour
{
  private static Dev instance;
  [Tooltip("Developer options")]
  [SerializeField]
  private DevOptions enable = new DevOptions()
  {
    logging = false
  };
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
  public static void Log<T>(T message)
  {
    if (instance != null && instance.enable.logging && instance.gameObject.activeInHierarchy)
    {
      Debug.Log(message.ToString());
    }
  }
}