using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
  public bool playerHasVisited { get; set; }
  public Vector3 spawnPoint = new Vector3(0, 0, 0);
  // Start is called before the first frame update
  void Start()
  {
    GameObject player = GameObject.FindWithTag("Player");
    if (spawnPoint != Vector3.zero)
    {
      player.transform.position = spawnPoint;
    }

    playerHasVisited = true;
  }

  // Update is called once per frame
  void Update()
  {

  }
}
