using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [ExecuteInEditMode]

    public Node[] neighbors;
    public Vector2[] ValidDirections;

    private void Start()
    {
        
        ValidDirections = new Vector2[neighbors.Length];
        for(int i = 0;i < neighbors.Length;i++)
        {
            Vector2 dir = (neighbors[i].gameObject.transform.localPosition - this.transform.localPosition).normalized;
            if(dir.x > 0 && dir.y == 0)
            {
                ValidDirections[i].x = 1;
                ValidDirections[i].y = 0;
            }
            if(dir.x < 0 && dir.y == 0)
            {
                ValidDirections[i].x = -1;
                ValidDirections[i].y = 0;
            }
            if(dir.x == 0 && dir.y > 0)
            {
                ValidDirections[i].x = 0;
                ValidDirections[i].y = 1;
            }
            if(dir.x == 0 && dir.y < 0)
            {
                ValidDirections[i].x = 0;
                ValidDirections[i].y = -1;
            }

        }
    }
}
