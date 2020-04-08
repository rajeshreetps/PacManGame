using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateBehaviour : MonoBehaviour
{
    public void OnAnimationExit()
   {
        if(this.GetComponentInParent<Ghost>().ghostMode == Ghost.GhostMode.Frighted)
        {
            this.GetComponentInParent<Ghost>().ghostMode = this.GetComponentInParent<Ghost>().PastGhostMode;
        }
    }
}
