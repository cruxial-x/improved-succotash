using UnityEngine;

internal class Dev
{
  public static void Log<T>(T message)
  {
    if (Debug.isDebugBuild)
    {
      Debug.Log(message.ToString());
    }
  }
}