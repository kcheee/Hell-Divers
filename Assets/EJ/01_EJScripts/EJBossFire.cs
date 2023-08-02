using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJBossFire : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward*10);
        Debug.DrawLine(transform.position, transform.forward*10);


    }
}
