using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class Stompery : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rgbd;
    [SerializeField]
    private Transform trnsf;

    public float _bouncy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (trnsf.position.y <= -10)
        {
            Debug.Log("wtf");
            trnsf.position = new Vector3(0, 5, 0);
        }
    }
    
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Ghosty"))
        {
            Debug.Log("testerestest");
            Destroy(other.gameObject);
            rgbd.velocity = new Vector3 (rgbd.velocity.x, _bouncy, rgbd.velocity.z);
        }
    }
}
