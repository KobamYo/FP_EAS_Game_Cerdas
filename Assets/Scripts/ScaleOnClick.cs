using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleOnClick : MonoBehaviour
{
    public float scaleSpeed = 0.1f;
    public float maxScale = 2.0f;
    public float minScale = 0.5f;
    public float normalScale = 1f;

    bool isScalingUp = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            isScalingUp = true;
            StartCoroutine(ScaleUp());
        }
        else if (Input.GetMouseButtonDown(1)) // Right mouse button
        {
            isScalingUp = false;
            StartCoroutine(ScaleDown());
        }
        else if (Input.GetMouseButtonDown(2)) // middle mouse click / scroll wheel
        {
            transform.localScale = new Vector3(1, 1, 1);    
        }
    }

     private IEnumerator ScaleUp()
    {
        while (transform.localScale.x < maxScale)
        {
            transform.localScale += new Vector3(scaleSpeed, scaleSpeed, scaleSpeed);
            yield return null;
        }
    }

    private IEnumerator ScaleDown()
    {
        while (transform.localScale.x > minScale)
        {
            transform.localScale -= new Vector3(scaleSpeed, scaleSpeed, scaleSpeed);
            yield return null;
        }
    }
}
