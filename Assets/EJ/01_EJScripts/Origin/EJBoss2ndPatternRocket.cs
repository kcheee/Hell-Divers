using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EJBoss2ndPatternRocket : MonoBehaviourPun
{
    float rocketSpeed;

    Rigidbody rb;


    public PhotonView tankPv;
    // Start is called before the first frame update
    void Start()
    {
        rocketSpeed = Random.Range(10, 20);
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * rocketSpeed, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position += transform.forward * rocketSpeed * Time.deltaTime;

        //??? 로켓헤드 방향
        transform.forward = rb.velocity.normalized;
    }
}
