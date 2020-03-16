using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Gamecontroller : MonoBehaviour
{
    public static Gamecontroller Instance;
   // public GameObject FoodObject;
    public TextMeshPro TxtScore;
    public TextMeshProUGUI txtScore;
    public int score;


    // Start is called before the first frame update
    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if(txtScore !=null)
            txtScore.text = score.ToString();

        if(TxtScore != null)
            TxtScore.text = score.ToString();
    }


}
