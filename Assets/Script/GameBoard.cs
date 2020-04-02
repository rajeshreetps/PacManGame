using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{

    public static GameBoard Instance;

    public static int BoardHeight=16;
    public static int BoardWidth =22;
    
    public GameObject[,] Board = new GameObject[BoardWidth, BoardHeight];
    public List<Node> objects;

    // Start is called before the first frame update
    void Start()
    {

        if(Instance == null)
            Instance = this;
        Node[] Nodes = FindObjectsOfType<Node>();
        foreach(var obj in Nodes)
        {
            Vector2 pos = obj.transform.localPosition;

            if(obj.tag == "Pettel" || obj.tag == "PowerPettel")
            {
                objects.Add( obj);
                Board[(int)pos.x, (int)pos.y] = obj.gameObject;
            }
        }       

    }
}
