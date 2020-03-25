using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateBehaviour : MonoBehaviour
{
    public void OnAnimationExit()

   {
        this.GetComponentInParent<Ghost>().ghostMode = this.GetComponentInParent<Ghost>().PastGhostMode;
    }
}
