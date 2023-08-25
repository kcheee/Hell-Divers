using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EJBoss2ndPatternRocket : MonoBehaviourPun
{
    float rocketSpeed;

    Rigidbody rb;
    public GameObject rocketExploImpactFactory;


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

    [PunRPC]
    void ShowRocketExploImpact(Vector3 pos, Vector3 normal, float waitTime)
    {
        GameObject rocketExploImpact = Instantiate(rocketExploImpactFactory);

        rocketExploImpact.transform.position = pos;
        rocketExploImpact.transform.localScale = Vector3.one * 3;
        rocketExploImpact.transform.forward = normal;

        StartCoroutine(wait(rocketExploImpact, waitTime));
    }

    IEnumerator wait(GameObject rocket, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }
}
