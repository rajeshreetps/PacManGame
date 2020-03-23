using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Playercontoller : MonoBehaviour
{
    

    float smooth = 5.0f;
    float tiltAngle = 60.0f;

    float tiltAroundZ;
    float tiltAroundX;
    float tiltAroundY;


    public enum Sides
    {Left,Right,Up,Down };

    public bool IsStop = true;
    public float speed = 100f;
    Vector3 velo = Vector3.zero;
    Vector3 rotate;
    Vector2 pos,Nextpos ;
    public Animator Anim;

    public GameBoard gameBoard;

    public Node CurrentNode,PreviousNode,TargetNode;

    public GameObject[] Ghosts;
    // Start is called before the first frame update
    void Start()
    {

        //main = this.GetComponent<MeshRenderer>().material.mainTexture;
        

        Node node = GetNodeAtPosition(transform.position);
        if(node != null)
        {
            CurrentNode = node;
        }

        ChangePosition(pos);
        pos = Vector2.zero;
    }

    Node GetNodeAtPosition(Vector2 pos)
    {
        GameObject tile = gameBoard.Board[(int)pos.x, (int)pos.y];

        if(tile != null)
        {
            return tile.GetComponent<Node>();
        }
        return null;

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangePosition(Vector2.left);
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow) )
        {
            ChangePosition(Vector2.right);

        }
        else if(Input.GetKeyDown(KeyCode.UpArrow) )
        {
            ChangePosition(Vector2.up);
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow) )
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

            }else
            {
                NextPos();
                transform.position += (Vector3)(pos * speed) * Time.deltaTime;
            }
        }

            this.GetComponent<Animator>().SetBool("IsStop", IsStop);



        // Rotate the cube by converting the angles into a quaternion.
        Quaternion target = Quaternion.Euler(tiltAroundX, tiltAroundY, tiltAroundZ);

            // Dampen towards the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, target, speed);
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
            Gamecontroller.Instance.score += 10;
        }
        if(collision.gameObject.tag == "PowerPettel")
        {
            collision.gameObject.SetActive(false);
            Gamecontroller.Instance.score += 100;
            for(int i = 0;i < Ghosts.Length;i++)
            {
                Ghost ghost = Ghosts[i].GetComponent<Ghost>();
                ghost.goastMode = Ghost.GoastMode.Frighted;
            }
        }
    }
}
 