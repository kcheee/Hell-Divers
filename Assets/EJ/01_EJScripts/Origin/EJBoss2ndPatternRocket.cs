using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EJBoss2ndPatternRocket : MonoBehaviourPun
{
    float rocketSpeed = Random.Range(10, 20);

    Transform rocketPos;
    public Rigidbody rb;


    public PhotonView tankPv;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += rocketPos.up * rocketSpeed * Time.deltaTime;
    }
}
