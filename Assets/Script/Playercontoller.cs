using UnityEngine;

public class Playercontoller : MonoBehaviour
{
    public static Playercontoller Instance;

    float tiltAroundZ;
    float tiltAroundX;
    float tiltAroundY;



    public bool IsStop = false,IsOut=false;
    public float speed = 100f;
    Vector3 rotate;
    public Vector2 pos,Nextpos ;

    public GameBoard gameBoard;

    public Node CurrentNode,PreviousNode,TargetNode;

    GameObject[] Ghosts;
    // Start is called before the first frame update
    void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    private void Start()
    {
        gameBoard = Gamecontroller.Instance.gameBoard;
        SetPlayer();
    }

    private void OnEnable()
    {
        SetPlayer();
    }

    void SetPlayer()
    {
        Ghosts = Gamecontroller.Instance.Ghosts;
        CurrentNode = null;
        PreviousNode = null;
        TargetNode = null;
        IsStop = true;
        IsOut = false;

        pos = Vector2.zero;
        Nextpos = Vector2.zero;

        tiltAroundY = 0f;
        tiltAroundZ = 0f;

        ChangePosition(pos);

        GetcurrentNode();

    }

    Node GetNodeAtPosition(Vector2 pos)
    {
        GameObject tile;
        if(gameBoard != null && gameBoard.Board[(int)Mathf.Round(pos.x), (int)Mathf.Round(pos.y)] != null)
        {
            tile = gameBoard.Board[(int)Mathf.Round(pos.x), (int)Mathf.Round(pos.y)];
            if(tile != null)
            {
                return tile.GetComponent<Node>();
            }
        }
        return null;

    }

    // Update is called once per frame
    void Update()
    {
        if(!IsOut && Gamecontroller.Instance.gameState == Gamecontroller.GameState.Running)
        {
            if(Input.GetKeyDown(KeyCode.LeftArrow))
            {
                ChangePosition(Vector2.left);
            }
            else if(Input.GetKeyDown(KeyCode.RightArrow))
            {
                ChangePosition(Vector2.right);

            }
            else if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                ChangePosition(Vector2.up);
            }
            else if(Input.GetKeyDown(KeyCode.DownArrow))
            {
                ChangePosition(Vector2.down);
            }

            //SetSprite();
            if(TargetNode != null && TargetNode != CurrentNode)
            {
                if(OverShotTarget())
                {
                    CurrentNode = TargetNode;
                    transform.position = CurrentNode.transform.position;

                    Node MoveToNode = CanMove(Nextpos);
                    if(MoveToNode != null)
                        pos = Nextpos;

                    if(MoveToNode == null)
                        MoveToNode = CanMove(pos);

                    if(MoveToNode != null)
                    {
                        TargetNode = MoveToNode;
                        PreviousNode = CurrentNode;
                        CurrentNode = null;
                    }
                    else
                    {
                        pos = Vector2.zero;
                        IsStop = true;
                    }

                }
                else
                {
                    NextPos();
                    transform.position += (Vector3)(pos * speed) * Time.deltaTime;
                }
            }


            // Rotate the cube by converting the angles into a quaternion.
            Quaternion target = Quaternion.Euler(tiltAroundX, tiltAroundY, tiltAroundZ);

            // Dampen towards the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, target, speed);
        }

        this.GetComponent<Animator>().SetBool("IsStop", IsStop);

        this.GetComponent<Animator>().SetBool("IsOver", IsOut);
    }

    public void GetcurrentNode()
    {
        Node node = GetNodeAtPosition(transform.position);

        if(node != null)
        {
            CurrentNode = node;
        }
    }

    float LenghfromNode(Vector2 TargetPos)
    {
        Vector2 vec = TargetPos - (Vector2)PreviousNode.transform.position;
        return vec.sqrMagnitude;
    }

    bool OverShotTarget()
    {
        float NodeToTarget = LenghfromNode(TargetNode.transform.position);
        float NodeToSelf = LenghfromNode(transform.position);
        return NodeToSelf > NodeToTarget;
    }

    void ChangePosition(Vector2 d)
    {
        if(d != pos)
            Nextpos = d;

        if(CurrentNode != null)
        {
            Node MoveToNnode = CanMove(d);
            if(MoveToNnode != null)
            {
                IsStop = false;
                pos = d;
                TargetNode = MoveToNnode;
                PreviousNode = CurrentNode;
                CurrentNode = null;
                MoveToDir(pos);
            }
            
        }
    }

    void MoveToDir(Vector3 d)
    {
        Node MoveToNode = CanMove(d);
        
        if(MoveToNode != null)
        {
            CurrentNode = MoveToNode;
        }
    }

    Node CanMove(Vector2 dir)
    {
        Node MoveToNode = null;
        if(CurrentNode != null)
        {
            for(int i = 0;i < CurrentNode.neighbors.Length;i++)
            {
                if(CurrentNode.ValidDirections[i] == dir)
                {
                    MoveToNode = CurrentNode.neighbors[i];
                    break;
                }
            }
        }
        return MoveToNode;
    }

    public void NextPos()
    {
        if(pos == Vector2.left)
        {
            tiltAroundY = 180;
            tiltAroundZ = 0f;
        }
        else if(pos == Vector2.right)
        {
            tiltAroundY = 0f;
            tiltAroundZ = 0f;
        }
        else if(pos == Vector2.up)
        {
            tiltAroundZ = 90f;
            tiltAroundY = 0f;
        }
        else if(pos == Vector2.down)
        {
            tiltAroundZ = 270;
            tiltAroundY = 0f;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
       if(collision.gameObject.tag == "Pettel")
        {
            collision.gameObject.SetActive(false);
            GameBoard.Instance.objects.Remove(collision.gameObject.GetComponent<Node>());
            Gamecontroller.score += 10;
        }
        if(collision.gameObject.tag == "PowerPettel")
        {
            collision.gameObject.SetActive(false);
            GameBoard.Instance.objects.Remove(collision.gameObject.GetComponent<Node>());
            Gamecontroller.score += 100;
            for(int i = 0;i < Ghosts.Length;i++)
            {
                Ghost ghost = Ghosts[i].GetComponent<Ghost>();
                ghost.ghostMode = Ghost.GhostMode.Frighted;
            }
        }
        if(GameBoard.Instance.objects.Count == 0)
        {
            RestartGame();
        }
        if(collision.gameObject.tag == "Ghost")
        {
            GameObject ghost = collision.gameObject;
            Debug.Log(ghost.GetComponent<Ghost>().ghostMode);
            if(ghost.GetComponent<Ghost>().ghostMode == Ghost.GhostMode.Chase || ghost.GetComponent<Ghost>().ghostMode == Ghost.GhostMode.Scatter)
            {
                Debug.Log("Player Die");
                pos = Vector3.zero;
                for(int i = 0;i < Ghosts.Length;i++)
                {
                    Ghost ghost1 = Ghosts[i].GetComponent<Ghost>();
                    ghost1.ghostMode = Ghost.GhostMode.Over;
                }
                IsStop = true;
                Invoke("GameOut", 1f);
            }
            else if(ghost.GetComponent<Ghost>().ghostMode == Ghost.GhostMode.Frighted)
            {
                ghost.GetComponent<Ghost>().MoveToNode = null;
                ghost.GetComponent<Ghost>().speed = 0f;
                ghost.GetComponent<Ghost>().ghostMode = Ghost.GhostMode.Die;
                ghost.GetComponent<Ghost>().GetComponent<SpriteRenderer>().enabled = false;
                ghost.GetComponent<Ghost>().GetComponent<CircleCollider2D>().enabled = false;
            }
        }
    }

    void GameOut()
    {
        for(int i = 0;i < Ghosts.Length;i++)
        {
            Ghosts[i].SetActive(false);
        }
        IsOut = true;
    }

    public void RestartGame()
    {
        Gamecontroller.Instance.RestartGame();
    }


}
 