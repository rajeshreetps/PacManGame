using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goast : MonoBehaviour
{
    float speed = 3.5f;
    Animator EyeEnim;
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

    float Distance = 100000f;
    public Vector3 PacDirection;
    public Node[] nodes;
    public Vector2[] Directions;

    public bool IsPlay=false;

    public void Start()
    {
        EyeEnim = this.GetComponentInChildren<Animator>();

        Node node = GetNodeAtPosition(transform.position);
        if(node != null)
        {
            CurrentNode = node;
            PreviousNode = CurrentNode;
        }

        Debug.Log(goastType);
        EyeEnim.SetInteger("Eyecode", 1);
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
                StartCoroutine(GoastMovement(3f,Vector3.right));
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
        IsPlay = true;

    }

    // Update is called once per frame
    void Update()
    {
        if(IsPlay)
        {
            GetcurrentNode();

            ChangePosition();

            Setdirection();

            SetOrientation();

            transform.position += Direction * speed * Time.deltaTime;
        }
    }

    public void SetOrientation()
    {
        Debug.Log(EyeEnim.GetInteger("Eyecode"));
        if(Direction == Vector3.left)
        {
            EyeEnim.SetInteger("Eyecode", 1);
        }
        if(Direction == Vector3.right)
        {
            EyeEnim.SetInteger("Eyecode", 2);
        }
        if(Direction == Vector3.up)
        {
            EyeEnim.SetInteger("Eyecode", 3);
        }
        if(Direction == Vector3.down)
        {
            EyeEnim.SetInteger("Eyecode", 4);
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
        GameObject tile = gameBoard.Board[(int)Mathf.Round(pos.x), (int)Mathf.Round(pos.y)];

        if(tile != null)
        {
            return tile.GetComponent<Node>();
        }
        return null;
    }

    void Setdirection()
    {
        if(MoveToNode != null && PreviousNode != null && CurrentNode != null && OverShotTarget())
        {
            CurrentNode = MoveToNode;

            CurrentNode.transform.position = MoveToNode.transform.position;

            PreviousNode = CurrentNode;

            MoveToNode = CanMove();

            Direction = NextDirection ;
        }
        
    }

    void ChangePosition()
    {
        if(MoveToNode == null )
        {
            MoveToNode = CanMove();

            PreviousNode = CurrentNode;
        }
    }


    Node CanMove()
    {
        if(CurrentNode != null)
        {
            nodes = CurrentNode.neighbors;
            Directions = CurrentNode.ValidDirections;
            /*Distance = 10000f;
            for(int i = 0;i < nodes.Length;i++)
            {
                if(Vector3.Distance(nodes[i].transform.position, PacMan.transform.position) < Distance)
                {
                    Distance = Vector3.Distance(nodes[i].transform.position, PacMan.transform.position);
                    MoveToNode = nodes[i];
                    NextDirection = Directions[i];
                }
            }*/
            int num = Random.Range(0, nodes.Length);
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


