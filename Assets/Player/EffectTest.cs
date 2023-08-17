using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectTest : MonoBehaviour
{
    Rigidbody rbody;
    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = transform.forward * Time.deltaTime * 50;
        rbody.AddForce(transform.forward * 10,ForceMode.Impulse);
    }
}
