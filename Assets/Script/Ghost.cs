using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Ghost : MonoBehaviour 
{

    float speed = 3.5f;
    float Distance = 100000f;
    int ScatterObjId;
    int Scatter1 = 7;
    int Scatter2 = 7;
    int Scatter3 = 5;
    int Scatter4 = 5;

    int Chase1 = 20;
    int Chase2 = 20;
    int Chase3 = 20;
    int Chase4 = 20;

    public List<Node> scatterPoints;


    public enum GoastMode
    {
        Chase,
        Scatter,
        Frighted
    }

    public GoastMode PastgoastMode;
    public GoastMode goastMode;

    public enum GoastType{
        Blinky,
        Inky,
        Pinky,
        Clyde
    }
    //[HideInInspector]
    public GoastType goastType;

    [HideInInspector]
    public GameObject PacMan;
    //[HideInInspector]
    public GameBoard gameBoard;

    public Node CurrentNode, PreviousNode,MoveToNode;
    public Vector3 Direction = Vector3.zero;

    public Vector3 NextDirection =Vector3.zero;

    public Vector3 PacDirection;
    public Node[] nodes;
    public Vector2[] Directions;

    public bool IsPlay=false;

    public GameObject[] EyeObjects;
    public GameObject Frighted;

    
    
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

    }

    // Update is called once per frame
    void Update()
    {
            GetcurrentNode();

            ChangePosition();

            ChangeMode();

            Setdirection();

            SetOrientation();

        transform.position += Direction * speed * UnityEngine.Time.deltaTime;
    }

    public void SetOrientation()
    {
        if(goastMode != GoastMode.Frighted)
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

            if(goastMode == GoastMode.Scatter)
            {
                MoveToNode = null;
            }
            else
            {
                MoveToNode = SetNextNode(PacMan);
                Direction = NextDirection;
                CanMove(Direction);
            }
            
        }
        
    }

    void ChangePosition()
    {
        if(CurrentNode == null)
        {
            Direction = Vector3.zero;
        }
        if(MoveToNode == null )
        {

            if(goastMode == GoastMode.Scatter)
            {
                //CanMove(scatterPoints[ScatterObjId].gameObject);

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
                    Direction = NextDirection;
                }

            }
            else
            {
                MoveToNode = SetNextNode(PacMan);
            }

            PreviousNode = CurrentNode;
        }
    }

    void ChangeMode()
    {
        if(goastMode == GoastMode.Scatter)
        {
            speed = 3.5f;
            if(!this.GetComponent<Animator>().enabled)
                this.GetComponent<Animator>().enabled = true;
            if(!this.GetComponent<SpriteRenderer>().enabled)
                this.GetComponent<SpriteRenderer>().enabled = true;
            if(Frighted.activeSelf)
                Frighted.SetActive(false);
            if(PastgoastMode != goastMode)
                PastgoastMode = goastMode;

        }
        else if(goastMode == GoastMode.Chase)
        {
            speed = 3.5f;
            if(!this.GetComponent<Animator>().enabled)
                this.GetComponent<Animator>().enabled =false;
            if(!this.GetComponent<SpriteRenderer>().enabled)
                this.GetComponent<SpriteRenderer>().enabled = true;
            if(Frighted.activeSelf)
                Frighted.SetActive(false);
            if(PastgoastMode != goastMode)
                PastgoastMode = goastMode;

        }
        else if(goastMode == GoastMode.Frighted)
        {
            speed = 2f;

            if(this.GetComponent<Animator>().enabled)
            {
                Direction = Direction * -1;
                CanMove(Direction);
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
       
    }

    Node SetNextNode(GameObject obj)
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