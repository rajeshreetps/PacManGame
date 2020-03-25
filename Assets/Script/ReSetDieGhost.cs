using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReSetDieGhost : MonoBehaviour
{

    public void RestartGhost(GameObject Ghost)
    {
        Ghost.SetActive(false);
        Ghost.SetActive(true);
    }
}
