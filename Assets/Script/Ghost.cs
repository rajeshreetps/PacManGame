using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ghost : MonoBehaviour
{
    public static Ghost ghostInstance;
    public float speed = 3.5f;
    float Distance = 100000f;
    public float Scatter1 = 7;
    public float Scatter2 = 7;
    public float Scatter3 = 5;
    public float Scatter4 = 5;

    public float Chase1 = 20;
    public float Chase2 = 20;
    public float Chase3 = 20;
    public float Chase4 = 20;

    public List<Node> scatterPoints;


    public enum GhostMode
    {
        Chase,
        Scatter,
        Frighted,
        Die,
        Over
    }

    public GhostMode PastGhostMode;
    public GhostMode ghostMode;

    public enum GoastType
    {
        Blinky,
        Inky,
        Pinky,
        Clyde
    }
    //[HideInInspector]
    public GoastType goastType;

    public Node GhostDiePoint;
    public Node GhostDiePoint1;
    public Node GhostDiePoint2;
    public Node GhostDieNode1;
    public Node GhostDieNode;
    public GameObject StartObject;

    [HideInInspector]
    public GameObject PacMan;
    //[HideInInspector]
    public GameBoard gameBoard;

    public Node SCurrentNode, SPreviousNode, SMoveToNode;
    public Node CurrentNode, PreviousNode, MoveToNode;
    public Vector3 Direction = Vector3.zero;

    public Vector3 NextDirection = Vector3.zero;

    public Vector3 PacDirection;
    public Node[] nodes;
    public Vector2[] Directions;

    public bool IsPlay = false;

    public GameObject[] EyeObjects;
    public GameObject Frighted;



    public void Start()
    {
        if(ghostInstance == null)
            ghostInstance = this;


        if(SCurrentNode == null)
        {
            SCurrentNode = CurrentNode;
        }
        if(SPreviousNode == null)
        {
            SPreviousNode = PreviousNode;
        }
        if(SMoveToNode == null)
        {
            SMoveToNode = MoveToNode;
        }

        

    }

    public void OnEnable()
    {
        Node node = GetNodeAtPosition(transform.position);
        if(node != null)
        {
            CurrentNode = node;
            PreviousNode = CurrentNode;
        }
        
        
        Scatter1 = 7;
        Scatter2 = 7;
        Scatter3 = 5;
        Scatter4 = 5;

        Chase1 = 20;
        Chase2 = 20;
        Chase3 = 20;
        Chase4 = 20;
        Direction = Vector3.zero;
        speed = 0f;
    }

    public void GhostStartMoving()
    {
        this.GetComponent<Animator>().enabled = true;
        this.GetComponent<SpriteRenderer>().enabled = true;
        Frighted.SetActive(false);
        GetComponent<Ghost>().GetComponent<CircleCollider2D>().enabled = true;
        if(ghostMode == GhostMode.Die || ghostMode == GhostMode.Over)
        {
            this.transform.position = StartObject.transform.position;
            CurrentNode = SCurrentNode;
            PreviousNode = SPreviousNode;
            MoveToNode = SMoveToNode;
            CheckingGoastTypeAfterDie();
        }
        else
        {
            CheckingGoastType();
        }

        ghostMode = GhostMode.Chase;
    }

    void CheckingGoastType()
    {
        switch(goastType)
        {
            case GoastType.Blinky:
                StartCoroutine(GoastMovement(0f, Vector3.left));
                break;
            case GoastType.Inky:
                StartCoroutine(GoastMovement(3f, Vector3.right));
                break;
            case GoastType.Pinky:
                StartCoroutine(GoastMovement(6f, Vector3.up));
                break;
            case GoastType.Clyde:
                StartCoroutine(GoastMovement(9f, Vector3.left));
                break;
        }

    }
    void CheckingGoastTypeAfterDie()
    {
        switch(goastType)
        {
            case GoastType.Blinky:
                StartCoroutine(GoastMovement(0f, Vector3.left));
                break;
            case GoastType.Inky:
                StartCoroutine(GoastMovement(3f, Vector3.right));
                break;
            case GoastType.Pinky:
                StartCoroutine(GoastMovement(6f, Vector3.up));
                break;
            case GoastType.Clyde:
                StartCoroutine(GoastMovement(9f, Vector3.left));
                break;
        }

    }

    public IEnumerator GoastMovement(float DelayTime, Vector3 dir)
    {
        yield return new WaitForSecondsRealtime(DelayTime);
        Direction = dir;
        speed = 3.5f;

    }

    // Update is called once per frame
    void Update()
    {
        if(Direction != Vector3.zero)
        {
            GetcurrentNode();

            ChangePosition();

            ChangeMode();

            Setdirection();

            SetOrientation();

            transform.position += Direction * speed * UnityEngine.Time.deltaTime;
        }
    }

    public void SetOrientation()
    {
        if(ghostMode != GhostMode.Frighted)
        {
            int index = 0;
            if(Direction == Vector3.left)
            {
                index = 0;
            }
            if(Direction == Vector3.right)
            {
                index = 1;
            }
            if(Direction == Vector3.up)
            {
                index = 2;
            }
            if(Direction == Vector3.down)
            {
                index = 3;
            }

            for(int i = 0;i < EyeObjects.Length;i++)
            {
                if(index == i)
                {
                    EyeObjects[i].SetActive(true);
                }
                else
                {
                    EyeObjects[i].SetActive(false);
                }

            }
        }
    }

    public void GetcurrentNode()
    {
        Node node = GetNodeAtPosition(transform.position);

        if(node != null)
        {
            CurrentNode = node;
        }
    }

    Node GetNodeAtPosition(Vector2 pos)
    {
        GameObject tile;
        if(gameBoard.Board[(int)Mathf.Round(pos.x), (int)Mathf.Round(pos.y)] != null)
        {
            tile = gameBoard.Board[(int)Mathf.Round(pos.x), (int)Mathf.Round(pos.y)];
            if(tile != null)
            {
                return tile.GetComponent<Node>();
            }
        }
        return CurrentNode;
    }

    void Setdirection()
    {
        if(MoveToNode != null && PreviousNode != null && CurrentNode != null && OverShotTarget())
        {
            CurrentNode = MoveToNode;

            CurrentNode.transform.position = MoveToNode.transform.position;

            PreviousNode = CurrentNode;

            if(ghostMode == GhostMode.Die)
            {
                if(CurrentNode == GhostDieNode)
                {
                    Direction = Vector3.zero;
                    speed = 0;
                    GhostDieNode.GetComponent<ReSetDieGhost>().RestartGhost(this.gameObject);
                }
                else if(CurrentNode == GhostDieNode1)
                {
                    MoveToNode = GhostDieNode;
                    Direction = Vector3.down;
                }
                else if(CurrentNode == GhostDiePoint)
                {
                    Debug.Log("Ghost Point");
                    MoveToNode = GhostDieNode1;
                    if(GhostDiePoint == GhostDiePoint1)
                        Direction = Vector3.left;
                    else if(GhostDiePoint == GhostDiePoint2)
                        Direction = Vector3.right;

                }
                else
                {
                    MoveToNode = SetNextNode(GhostDiePoint.gameObject);
                    Direction = NextDirection;
                    CanMove(Direction, CurrentNode);
                }
            }
            else
            {
                MoveToNode = null;
                CanMove(Direction,CurrentNode);
            }


        }

    }



    void ChangePosition()
    {
        if(CurrentNode == null)
        {
            Direction = Vector3.zero;
        }
        if(MoveToNode == null)
        {

            if(ghostMode == GhostMode.Scatter)
            {
                MoveToNode = SetNextNode(scatterPoints[0].gameObject);
                if(scatterPoints.Exists(asd => asd == CurrentNode))
                {
                    int nodeid = scatterPoints.FindIndex(asd => asd == CurrentNode);
                    Node Scatternode = scatterPoints[nodeid];

                    int NextPoint = nodeid + 1;
                    if(NextPoint == scatterPoints.Count)
                    {
                        NextPoint = 0;
                    }

                    int Movenodeid = scatterPoints.FindIndex(asd => asd == scatterPoints[NextPoint]);
                    Node MoveScatternode = scatterPoints[Movenodeid];

                    MoveToNode = MoveScatternode;

                    int dirid = Array.IndexOf(Scatternode.neighbors, MoveScatternode);
                    NextDirection = Scatternode.ValidDirections[dirid];
                }

            }
            else if(ghostMode == GhostMode.Die)
            {
                float Distance1 = Vector3.Distance(CurrentNode.transform.position, GhostDiePoint1.transform.position);
                float Distance2 = Vector3.Distance(CurrentNode.transform.position, GhostDiePoint2.transform.position);
                if(Distance1 > Distance2)
                    GhostDiePoint = GhostDiePoint1;
                else
                    GhostDiePoint = GhostDiePoint2;
                MoveToNode = SetNextNode(GhostDiePoint.gameObject);
            }
            else
            {
                MoveToNode = SetNextNode(PacMan);
                
            }
            Direction = NextDirection;

            CanMove(Direction);

            PreviousNode = CurrentNode;
        }
    }

    void ChangeMode()
    {
        if(ghostMode == GhostMode.Scatter)
        {
            speed = 3.5f;
            if(goastType == GoastType.Blinky)
            {
                Scatter1 -= Time.deltaTime;
                if(Scatter1 < 0)
                {                   
                    Scatter1 = 7;
                    ghostMode = GhostMode.Chase;
                }          
            }
            if(goastType == GoastType.Pinky)
            {
                Scatter2 -= Time.deltaTime;
                if(Scatter2 < 0)
                {
                    Scatter2 = 7;
                    ghostMode = GhostMode.Chase;
                }
            }
            if(goastType == GoastType.Inky)
            {
                Scatter3 -= Time.deltaTime;
                if(Scatter3 < 0)
                {
                    Scatter3 = 5;
                    ghostMode = GhostMode.Chase;
                }
            }
            if(goastType == GoastType.Clyde)
            {
                Scatter4 -= Time.deltaTime;
                if(Scatter4 < 0)
                {
                    Scatter4 =5;
                    ghostMode = GhostMode.Chase;
                }
            }
            if(!this.GetComponent<Animator>().enabled)
                this.GetComponent<Animator>().enabled = true;
            if(!this.GetComponent<SpriteRenderer>().enabled)
                this.GetComponent<SpriteRenderer>().enabled = true;
            if(Frighted.activeSelf)
            {
                Frighted.SetActive(false);
                GetComponent<Ghost>().GetComponent<CircleCollider2D>().enabled = true;
            }
            if(PastGhostMode != ghostMode)
                PastGhostMode = ghostMode;

        }
        else if(ghostMode == GhostMode.Chase)
        {
            if(goastType == GoastType.Blinky)
            {
                Chase1 -= Time.deltaTime;
                if(Chase1 < 0)
                {
                    Chase1 = 20;
                    MoveToNode = null;
                    ghostMode = GhostMode.Scatter;
                   
                }
            }
            if(goastType == GoastType.Pinky)
            {
                Chase2 -= Time.deltaTime;
                if(Chase2 < 0)
                {
                    Chase2 = 20;
                    MoveToNode = null;
                    ghostMode = GhostMode.Scatter;

                }
            }
            if(goastType == GoastType.Inky)
            {
                Chase3 -= Time.deltaTime;
                if(Chase3 < 0)
                {
                    Chase3 = 20;
                    MoveToNode = null;
                    ghostMode = GhostMode.Scatter;

                }
            }
            if(goastType == GoastType.Clyde)
            {
                Chase4 -= Time.deltaTime;
                if(Chase4 < 0)
                {
                    Chase4 = 20;
                    MoveToNode = null;
                    ghostMode = GhostMode.Scatter;

                }
            }
            speed = 3.5f;
            if(!this.GetComponent<Animator>().enabled)
                this.GetComponent<Animator>().enabled = true;
            if(!this.GetComponent<SpriteRenderer>().enabled)
                this.GetComponent<SpriteRenderer>().enabled = true;
            if(Frighted.activeSelf)
            {
                Frighted.SetActive(false);
                GetComponent<Ghost>().GetComponent<CircleCollider2D>().enabled = true;
            }
            if(PastGhostMode != ghostMode)
                PastGhostMode = ghostMode;

        }
        else if(ghostMode == GhostMode.Frighted)
        {
            speed = 2f;

            if(this.GetComponent<Animator>().enabled)
            {
                this.GetComponent<Animator>().enabled = false;
            }
            if(this.GetComponent<SpriteRenderer>().enabled)
                this.GetComponent<SpriteRenderer>().enabled = false;
            for(int i = 0;i < EyeObjects.Length;i++)
            {
                EyeObjects[i].SetActive(false);
            }
            if(!Frighted.activeSelf)
                Frighted.SetActive(true);
        }
        else if(ghostMode == GhostMode.Die)
        {

            if(Frighted.activeSelf)
            {
                speed = 0;
                Frighted.SetActive(false);
                Invoke("GotoGhostHouse", 0.5f);
            }

        }
        else
        {
            Direction = Vector3.zero;
        }

    }

    void GotoGhostHouse()
    {
        speed = 10f;

    }
    Node SetNextNode(GameObject obj)
    {
        if(CurrentNode != null)
        {
            nodes = CurrentNode.neighbors;
            Directions = CurrentNode.ValidDirections;
            if(ghostMode == GhostMode.Die || ghostMode == GhostMode.Scatter)
            {
                Distance = 10000f;
                for(int i = 0;i < nodes.Length;i++)
                {
                    if(Vector3.Distance(nodes[i].transform.position, obj.transform.position) < Distance)
                    {
                        Distance = Vector3.Distance(nodes[i].transform.position, obj.transform.position);
                        MoveToNode = nodes[i];
                        NextDirection = Directions[i];
                    }
                }
            }
            else
            {
                int index;

                if(nodes.Length == 2)
                {
                    index = Array.IndexOf(Directions, Direction);
                    if(index == -1)
                        index = Random.Range(0, Directions.Length);
                }
                else 
                    index = Random.Range(0, Directions.Length);

                MoveToNode = nodes[index];
                NextDirection = Directions[index];

            }
        }
        return MoveToNode;
    }

    void CanMove(Vector3 dir)
    {
        if(CurrentNode != null)
        {
            Node movetonode = null;
            nodes = CurrentNode.neighbors;
            Directions = CurrentNode.ValidDirections;
            Distance = 10000f;
            for(int i = 0;i < nodes.Length;i++)
            {
                if((Vector3)Directions[i] == dir)
                {
                    movetonode = nodes[i];
                    NextDirection = Directions[i];
                }
            }
            if(movetonode == null)
            {
                movetonode = nodes[0];
                NextDirection = Directions[0];
            }

            MoveToNode = movetonode;
        }
    }

    void CanMove(Vector3 dir,Node CurrentNode)
    {
        if(CurrentNode != null)
        {
            bool IsValid = false;
            nodes = CurrentNode.neighbors;
            Directions = CurrentNode.ValidDirections;
            Distance = 10000f;
            for(int i = 0;i < nodes.Length;i++)
            {
                if((Vector3)Directions[i] == dir)
                {
                    IsValid = true;
                    break;
                }
            }
            if(!IsValid)
            {
                MoveToNode = nodes[0];
                Direction = Directions[0];
            }

            
        }
    }


    bool OverShotTarget()
    {
        float NodeToTarget = LenghfromNode(MoveToNode.transform.position);
        float NodeToSelf = LenghfromNode(this.transform.position);
        return NodeToSelf > NodeToTarget;

    }

    float LenghfromNode(Vector2 TargetPos)
    {
        Vector2 vec = TargetPos - (Vector2)PreviousNode.transform.position;
        return vec.sqrMagnitude;
    }

}