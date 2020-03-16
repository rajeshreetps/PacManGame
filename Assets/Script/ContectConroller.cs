using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContectConroller : MonoBehaviour
{
    public Playercontoller playercontoller;

    private void OnCollisionEnter2D(Collision2D collision)
    {

        Debug.Log(collision.collider.name);
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
       
    }

}
