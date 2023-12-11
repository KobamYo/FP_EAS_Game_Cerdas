using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMove : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    [SerializeField]
    private Rigidbody rgbd;

    [SerializeField]
    private float _forceMagnitude;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float inputHorizontal = Input.GetAxis("Horizontal");
        float inputVertical =  Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(inputHorizontal, 0, inputVertical);

        // _transform.position += moveDirection * _speed * Time.deltaTime;

        rgbd.AddForce(moveDirection * _forceMagnitude * Time.deltaTime);
    }
}
