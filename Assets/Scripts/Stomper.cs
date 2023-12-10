using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stomper : MonoBehaviour
{   
    [SerializeField]
    private Rigidbody rgbd;
    [SerializeField]
    private float bouncer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Ghost"))
        {
            Debug.Log("testing if this is fine");
            Destroy(other.gameObject);
            rgbd.velocity = new Vector3(rgbd.velocity.x, bouncer, rgbd.velocity.z);
        }
    }
}
