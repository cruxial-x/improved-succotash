using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
  public Vector3 spawnPoint = new Vector3(14.8197899f, 4.92129803f, 0);
  private Vector3 roomCenter = new Vector3(0, 0, 0);
  // Start is called before the first frame update
  void Start()
  {
    transform.position = roomCenter;
    GameObject player = GameObject.FindWithTag("Player");
    player.transform.position = spawnPoint;
  }

  // Update is called once per frame
  void Update()
  {

  }
}
