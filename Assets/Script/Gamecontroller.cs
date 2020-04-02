using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gamecontroller : MonoBehaviour
{
    GameObject Player;
    int LifeCount = 3;

    public static int score;
    public static Gamecontroller Instance;
    // public GameObject FoodObject;
    public enum GameState
    {
        Ready,
        Running,
        GameEnd,
        GameOver,
        GameWin
    }
    [Space]
    public GameState gameState;
    [Space]
    public TextMeshPro TxtScore;
    [Space]
    public TextMeshProUGUI txtScore;
    [Space]
    public GameBoard gameBoard;
    [Space]
    public GameObject playerprefeb;
    [Space]
    public GameObject ObjReady;
    [Space]
    public GameObject ObjGameWin;
    [Space]
    public GameObject ObjGameOver;
    [Space]
    public GameObject ObjBlack;
    [Space]
    public Camera Camera;
    [Space]
    public GameObject[] LifeObject;
    [Space]
    public GameObject[] Ghosts;

    // Start is called before the first frame update

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }
    void Start()
    {
        StartCoroutine("GameStart");
    }


    public IEnumerator GameStart()
    {
        LifeCount -= 1;

        for(int i = 0;i < LifeObject.Length;i++)
        {
            if(i < LifeCount)
                LifeObject[i].SetActive(true);
            else
                LifeObject[i].SetActive(false);
        }
        yield return new WaitForSecondsRealtime(0.2f);
        ObjBlack.SetActive(true);

        if(LifeCount == -1)
        {
            if(gameBoard.objects.Count > 0)
            {

                gameState = GameState.GameOver;
                ObjGameOver.SetActive(true);
            }

            yield return new WaitForSecondsRealtime(1f);

        }
        else if(gameState == GameState.GameEnd || gameState==GameState.Ready)
        {
            gameState = GameState.Ready;
            if(Player == null)
                Player = Instantiate(playerprefeb, new Vector3(8f, 0f, 0f), new Quaternion(0f, 0f, 0f, 0f));
            for(int i = 0;i < 5;i++)
            {
                ObjReady.SetActive(!ObjReady.activeSelf);
                yield return new WaitForSecondsRealtime(0.5f);
            }
            ObjReady.SetActive(false);
            yield return new WaitForSecondsRealtime(0.5f);

            gameState = GameState.Running;

            for(int i = 0;i < Ghosts.Length;i++)
            {
                Ghosts[i].SetActive(true);
                Ghosts[i].GetComponent<Ghost>().GhostStartMoving();
            }
        }
    }

    public void RestartGame()
    {
        ObjBlack.SetActive(false);
        Destroy(Player);
        Player = null;
        for(int i = 0;i < Ghosts.Length;i++)
        {
            Ghosts[i].SetActive(false);
        }
        if(gameBoard.objects.Count == 0)
        {
            gameState = GameState.GameWin;
            ObjGameWin.SetActive(true);
        }
        else
        {
            gameState = GameState.GameEnd;
        }
        StartCoroutine("GameStart");
    }

    private void Update()
    {
        if(txtScore !=null)
            txtScore.text = score.ToString();

        if(TxtScore != null)
            TxtScore.text = score.ToString();

        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if(hit.collider != null && hit.collider.name == "Exit")
            {
                Application.Quit();

            }
        }
    }

    public void GameWin()
    {
        gameState = GameState.GameWin;
            ObjGameWin.SetActive(true);
    }
}
