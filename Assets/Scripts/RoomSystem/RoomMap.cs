using UnityEngine;
using UnityEngine.Tilemaps;

public class Minimap
{
  private Room room;
  private Tilemap[] tilemaps;
  private int[] initialLayers;
  private SpriteRenderer[] spriteRenderers;

  public Minimap(GameObject roomObject)
  {
    room = roomObject.GetComponent<Room>();
    this.tilemaps = roomObject.GetComponentsInChildren<Tilemap>();
    this.spriteRenderers = roomObject.GetComponentsInChildren<SpriteRenderer>();
    this.initialLayers = new int[tilemaps.Length];

    for (int i = 0; i < tilemaps.Length; i++)
    {
      initialLayers[i] = tilemaps[i].gameObject.layer;
    }

    SetupEdges(roomObject.transform, room.minEdgePos, room.maxEdgePos);
  }

  private void SetupEdges(Transform transform, Vector3 minEdgePos, Vector3 maxEdgePos)
  {
    // Create a LineRenderer for each edge of the box
    for (int i = 0; i < 4; i++)
    {
      GameObject edge = new GameObject("Edge" + i)
      {
        layer = LayerMask.NameToLayer("Mapped")
      };
      edge.transform.SetParent(transform);
      LineRenderer lineRenderer = edge.AddComponent<LineRenderer>();

      // Set the width of the line
      lineRenderer.startWidth = 0.50f;
      lineRenderer.endWidth = 0.50f;

      // Set the color of the line
      lineRenderer.material = new Material(Shader.Find("Unlit/Color"))
      {
        color = Color.black
      };

      // Set the positions of the line
      Vector3 startPos = Vector3.zero;
      Vector3 endPos = Vector3.zero;
      switch (i)
      {
        case 0:
          startPos = minEdgePos;
          endPos = new Vector3(minEdgePos.x, maxEdgePos.y, minEdgePos.z);
          break;
        case 1:
          startPos = new Vector3(minEdgePos.x, maxEdgePos.y, minEdgePos.z);
          endPos = maxEdgePos;
          break;
        case 2:
          startPos = maxEdgePos;
          endPos = new Vector3(maxEdgePos.x, minEdgePos.y, maxEdgePos.z);
          break;
        case 3:
          startPos = new Vector3(maxEdgePos.x, minEdgePos.y, maxEdgePos.z);
          endPos = minEdgePos;
          break;
      }
      lineRenderer.SetPosition(0, startPos);
      lineRenderer.SetPosition(1, endPos);
    }
  }

  public void Hide()
  {
    if (tilemaps != null)
    {
      for (int i = 0; i < tilemaps.Length; i++)
      {
        if (tilemaps[i].gameObject.layer == LayerMask.NameToLayer("Ground") ||
            tilemaps[i].gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
          tilemaps[i].gameObject.layer = room.isVisited ? LayerMask.NameToLayer("Mapped") : LayerMask.NameToLayer("Hidden");
        }
        else
        {
          tilemaps[i].gameObject.SetActive(false);
        }
      }
    }
    if (spriteRenderers != null)
      foreach (SpriteRenderer spriteRenderer in spriteRenderers)
      {
        if (spriteRenderer.gameObject.layer != LayerMask.NameToLayer("Hidden") &&
            spriteRenderer.gameObject.layer != LayerMask.NameToLayer("Mapped"))
        {
          spriteRenderer.enabled = false;
        }
      }
  }

  public void Show()
  {
    if (tilemaps != null)
    {
      for (int i = 0; i < tilemaps.Length; i++)
      {
        if (tilemaps[i].gameObject.layer == LayerMask.NameToLayer("Hidden") ||
            tilemaps[i].gameObject.layer == LayerMask.NameToLayer("Mapped"))
        {
          tilemaps[i].gameObject.layer = initialLayers[i];
        }
        else
        {
          tilemaps[i].gameObject.SetActive(true);
        }
      }
    }
    if (spriteRenderers != null)
      foreach (SpriteRenderer spriteRenderer in spriteRenderers)
      {
        spriteRenderer.enabled = true;
      }

  }
}