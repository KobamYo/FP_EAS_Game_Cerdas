using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class GhostSearch : MonoBehaviour
{
    [SerializeField]
    private Transform player;
    public NavMeshAgent ghosty;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ghosty.SetDestination(player.position);
    }


}
