using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Camera>().nearClipPlane = 20;
        GetComponent<Camera>().farClipPlane = 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
