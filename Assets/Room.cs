using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
  public Vector3 spawnPoint = new Vector3(0, 0, 0);
  public Camera roomCamera { get; private set; }
  public List<Door> doors = new List<Door>();
  // Start is called before the first frame update
  void Start()
  {
    GameObject player = GameObject.FindWithTag("Player");
    if (spawnPoint != Vector3.zero)
    {
      player.transform.position = spawnPoint;
    }
    roomCamera = GetComponentInChildren<Camera>();
  }

  // Update is called once per frame
  void Update()
  {

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