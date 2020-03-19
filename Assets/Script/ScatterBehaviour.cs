using UnityEngine;

public interface ScatterBehaviour
{
    void TopRightCornerPath();
    void TopLeftCornerPath();
    void BottomRightCornerPath();
    void BottomLeftCornerpath();
}

public class ScatterPoints
{
    public GameObject[] points;
}
