using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goast : MonoBehaviour
{

    float speed = 3.5f;
    float Distance = 100000f;
    int ScatterObjId;
    public float Scatter1 = 7;
    float Scatter2 = 7;
    float Scatter3 = 5;
    float Scatter4 = 5;

    public float Chase1 = 20;
    float Chase2 = 20;
    float Chase3 = 20;
    float Chase4 = 20;

    public List<Node> scatterPoints;


    public enum GoastMode
    {
        Chase,
        Scatter,
        Frighted
    }

    public GoastMode goastMode;

    public enum GoastType
    {
        Blinky,
        Pinky,
        Inky,
        Clyde
    }
    //[HideInInspector]
    public GoastType goastType;

    [HideInInspector]
    public GameObject PacMan;
    //[HideInInspector]
    public GameBoard gameBoard;

    public Node CurrentNode, PreviousNode, MoveToNode;
    public Vector3 Direction = Vector3.zero;

    public Vector3 NextDirection = Vector3.zero;

    public Vector3 PacDirection;
    public Node[] nodes;
    public Vector2[] Directions;

    public bool IsPlay = false;

    public GameObject[] EyeObjects;



    public void Start()
    {

        Node node = GetNodeAtPosition(transform.position);
        if(node != null)
        {
            CurrentNode = node;
            PreviousNode = CurrentNode;
        }


        CheckingGoastType();
    }

    void CheckingGoastType()
    {
        switch(goastType)
        {
            case GoastType.Blinky:
                StartCoroutine(GoastMovement(0f, Vector3.left));
                break;
            case GoastType.Pinky:
                StartCoroutine(GoastMovement(6f, Vector3.up));
                break;
            case GoastType.Inky:
                StartCoroutine(GoastMovement(3f, Vector3.right));
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

    }

    // Update is called once per frame
    void Update()
    {

        GetcurrentNode();

        ChangePosition();

        Setdirection();

        SetOrientation();

        transform.position += Direction * speed * UnityEngine.Time.deltaTime;

        ChangeMode();



    }

    public void SetOrientation()
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
        GameObject tile = null;

        if(gameBoard.Board[(int)pos.x, (int)pos.y] != null)
        {
            tile = gameBoard.Board[(int)pos.x, (int)pos.y];
            return tile.GetComponent<Node>();
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

            if(goastMode == GoastMode.Scatter)
            {
                MoveToNode = null;
            }
            else
            {
                MoveToNode = CanMove(PacMan);
            }

            Direction = NextDirection;
        }

    }

    void ChangePosition()
    {
        if(MoveToNode == null)
        {
            if(goastMode == GoastMode.Scatter)
            {
                CanMove(scatterPoints[ScatterObjId].gameObject);

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
                else
                {
                    nodes = CurrentNode.neighbors;
                    Directions = CurrentNode.ValidDirections;

                    Distance = 10000f;
                    for(int i = 0;i < nodes.Length;i++)
                    {
                        if(Vector3.Distance(nodes[i].transform.position, scatterPoints[ScatterObjId].gameObject.transform.position) < Distance)
                        {
                            Distance = Vector3.Distance(nodes[i].transform.position, scatterPoints[ScatterObjId].gameObject.transform.position);
                            MoveToNode = nodes[i];
                            NextDirection = Directions[i];
                        }

                    }

                }

                Direction = NextDirection;
            }
            else
            {
               
                MoveToNode = CanMove(PacMan);
            }

                PreviousNode = CurrentNode;
            }
        }

        void ChangeMode()
        {
            if(goastType == GoastType.Blinky)
            {

                if(goastMode == GoastMode.Scatter)
                {
                Scatter1 -= Time.deltaTime;
                if(Scatter1 < 0)
                    {
                        Debug.Log("Blinky Chase mode start");
                        Scatter1 = 7;
                        goastMode = GoastMode.Chase;
                    }
                }
                else if(goastMode == GoastMode.Chase)
                {
                Chase1 -= Time.deltaTime;
                if(Chase1 < 0)
                    {
                        Debug.Log("Blinky Scatter mode start");
                        Chase1 = 20;
                        goastMode = GoastMode.Scatter;
                    }
                }
            }
            if(goastType == GoastType.Pinky)
            {
                if(goastMode == GoastMode.Scatter)
                {
                    Scatter2 -= Time.deltaTime;
                    if(Scatter2 < 0)
                    {
                        Debug.Log("Pinky Chase mode start");
                        Scatter2 = 7;
                        goastMode = GoastMode.Chase;
                    }
                }
                else if(goastMode == GoastMode.Chase)
                {
                    Chase2 -= Time.deltaTime;
                    if(Chase2 < 0)
                    {
                        Debug.Log("Pinky Scatter mode start");
                        Chase2 = 20;
                        goastMode = GoastMode.Scatter;
                    }
                }
            }

            if(goastType == GoastType.Inky)
            {
                if(goastMode == GoastMode.Scatter)
                {
                    Scatter3 -= Time.deltaTime;
                    if(Scatter3 < 0)
                    {
                        Debug.Log("Inky Chase mode start");
                        Scatter3 = 7;
                        goastMode = GoastMode.Chase;
                    }
                }
                else if(goastMode == GoastMode.Chase)
                {
                    Chase3 -= Time.deltaTime;
                    if(Chase3 < 0)
                    {
                        Debug.Log("Inky Scatter mode start");
                        Chase3 = 20;
                        goastMode = GoastMode.Scatter;
                    }
                }
            }
            if(goastType == GoastType.Clyde)
            {
                if(goastMode == GoastMode.Scatter)
                {
                    Scatter4 -= Time.deltaTime;
                    if(Scatter4 < 0)
                    {
                        Debug.Log("Clyde Chase mode start");
                        Scatter4 = 7;
                        goastMode = GoastMode.Chase;
                    }
                }
                else if(goastMode == GoastMode.Chase)
                {
                    Chase4 -= Time.deltaTime;
                    if(Chase4 < 0)
                    {
                        Debug.Log("Clyde Scatter mode start");
                        Chase4 = 20;
                        goastMode = GoastMode.Scatter;
                    }
                }
            }
        }

        Node CanMove(GameObject obj)
        {
            if(CurrentNode != null)
            {
                nodes = CurrentNode.neighbors;
                Directions = CurrentNode.ValidDirections;
                /* Distance = 10000f;
                 for(int i = 0;i < nodes.Length;i++)
                 {
                     if(Vector3.Distance(nodes[i].transform.position, obj.transform.position) < Distance)
                     {
                         Distance = Vector3.Distance(nodes[i].transform.position, obj.transform.position);
                         MoveToNode = nodes[i];
                         NextDirection = Directions[i];
                     }
                 }*/
                int num = UnityEngine.Random.Range(0, nodes.Length);
                MoveToNode = nodes[num];
                NextDirection = Directions[num];

            }
            return MoveToNode;
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


        /*Vector3 GetdirectionOfPacMan(Vector3 position)
        {
            Vector3 pos = Vector3.zero;
            Vector3 Direction = this.transform.position - position;
            if(Direction.x > 0)
            {
                pos = Vector3.right;
            }

            if(Direction.x < 0)
            {
                pos = Vector3.left;
            }

            if(Direction.z > 0)
            {
                pos = Vector3.up;
            }

            if(Direction.z < 0)
            {
                pos = Vector3.down;
            }

            return pos;
        }*/
    }


