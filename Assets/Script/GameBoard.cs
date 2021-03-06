﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{

    public static GameBoard Instance;

    public static int BoardHeight=16;
    public static int BoardWidth =22;
    
    public GameObject[,] Board = new GameObject[BoardWidth, BoardHeight];

    // Start is called before the first frame update
    void Start()
    {

        if(Instance == null)
            Instance = this;
        GameObject[] objects = FindObjectsOfType<GameObject>();
        foreach(var obj in objects)
        {
            Vector2 pos = obj.transform.localPosition;
            if(obj.name != "Player")
            {
                Board[(int)pos.x, (int)pos.y] = obj;
            }
        }       

    }
}
