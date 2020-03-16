using UnityEngine;
[ExecuteInEditMode]
public class MyPlayerEditor : MonoBehaviour
{
    void Start()
    {
        RectTransform[] lstobjects = this.GetComponentsInChildren<RectTransform>() ;
        BoxCollider2D[] lst2DCol = this.GetComponentsInChildren<BoxCollider2D>();

        for(int i = 0;i < lstobjects.Length;i++)
        {
            if(lstobjects[i].gameObject != this.gameObject)
            {
                Debug.Log(lstobjects[i].name + " " + lstobjects[i].sizeDelta);
                lst2DCol[i].size = lstobjects[i].sizeDelta;
            }
        }
    }


}