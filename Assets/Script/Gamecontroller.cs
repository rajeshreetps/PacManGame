using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gamecontroller : MonoBehaviour
{
    public static Gamecontroller Instance;
   // public GameObject FoodObject;
    public TextMeshPro TxtScore;
    public TextMeshProUGUI txtScore;
    public int score;
    public int LifeCount = 3;
    public GameBoard gameBoard;
    public GameObject[] LifeObject;
    public GameObject playerprefeb;
    public GameObject Player;
    public GameObject[] Ghosts;
    //public Camera Camera;

    // Start is called before the first frame update

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }
    void Start()
    {
        GameStart();
    }


    void GameStart()
    {
        LifeCount -= 1;

        for(int i = 0;i < LifeObject.Length;i++)
        {
            if(i < LifeCount)
                LifeObject[i].SetActive(true);
            else
                LifeObject[i].SetActive(false);
        }
        if(LifeCount == -1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
       // Camera.gameObject.SetActive(true);
        for(int i = 0;i < Ghosts.Length;i++)
        {
            Ghosts[i].SetActive(true);
        }
        if(Player == null)
            Player = Instantiate(playerprefeb, new Vector3(8f, 0f, 0f),new Quaternion(0f,0f,0f,0f));
    }


    public void RestartGame()
    {
        //Camera.gameObject.SetActive(false);
        Destroy(Player);
        Player = null;
        for(int i = 0;i < Ghosts.Length;i++)
        {
            Ghosts[i].SetActive(false);
        }
        GameStart();
    }

    private void Update()
    {
        if(txtScore !=null)
            txtScore.text = score.ToString();

        if(TxtScore != null)
            TxtScore.text = score.ToString();

        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if(hit.collider != null && hit.collider.name == "Exit")
            {
                Application.Quit();

            }
        }
    }


}
