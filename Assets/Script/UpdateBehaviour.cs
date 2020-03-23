using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateBehaviour : MonoBehaviour
{
    public void OnAnimationExit()

   {
        this.GetComponentInParent<Ghost>().goastMode = this.GetComponentInParent<Ghost>().PastgoastMode;
    }
}
