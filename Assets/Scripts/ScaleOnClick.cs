using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleOnClick : MonoBehaviour
{
    public float scaleSpeed = 0.1f;
    public float maxScale = 2.0f;
    public float minScale = 0.5f;

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
            // StartCoroutine(ScaleUp());
            Debug.Log("dsdsdasdad");
            
        }
        else if (Input.GetMouseButtonDown(1)) // Right mouse button
        {
            isScalingUp = false;
            // StartCoroutine(ScaleDown());
            Debug.Log("testestetsa");
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            isScalingUp = true;
            // StartCoroutine(ScaleUp());
            Debug.Log("dsdsdasdad");
            
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            isScalingUp = false;
            // StartCoroutine(ScaleDown());
            Debug.Log("testestetsa");
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
