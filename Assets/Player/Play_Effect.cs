using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Play_Effect : MonoBehaviour
{
    Vector3 point;
    Vector3 dir;
    float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += dir * speed * Time.deltaTime;
        
        //if()
    }
}
