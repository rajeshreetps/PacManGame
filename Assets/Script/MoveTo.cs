using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveTo : MonoBehaviour
{
    public Transform Player;

    public float time = 5f;

    Transform Curtarget;

    Transform target;

    bool IsStart = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartFlowEnemy(time));

    }

    IEnumerator StartFlowEnemy(float time)
    {
        yield return new WaitForSecondsRealtime(time);

       // target = Gamecontroller.Instance.GetTaget();

        Curtarget = target;

        IsStart = true;
    }


    // Update is called once per frame
    void Update()
    {
        if(target == null)
            return;

        if(IsStart)
        {
            NavMeshAgent nav = GetComponent<NavMeshAgent>();

            nav.SetDestination(target.position);
        }

        if(Vector3.Distance(transform.position, target.position) < 1f || !target.gameObject.activeSelf)
        {
            IsStart = false;

            StartCoroutine(StartFlowEnemy(0));
        }

        if(Vector3.Distance(transform.position, Player.position) < 50f)
        {
            Curtarget = Player;
        }
        else if(Vector3.Distance(transform.position, Player.position) > 60f)
        {
            Curtarget = target;
        }
    }
}
