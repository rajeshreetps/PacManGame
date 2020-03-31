using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateBehaviour : MonoBehaviour
{
    public void OnAnimationExit()

   {
        Debug.Log("Animation Exit");
        if(this.GetComponentInParent<Ghost>().ghostMode == Ghost.GhostMode.Frighted)
        {
            this.GetComponentInParent<Ghost>().ghostMode = this.GetComponentInParent<Ghost>().PastGhostMode;
        }
    }
}
