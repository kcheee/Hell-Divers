using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EJBoss2ndPatternRocket : MonoBehaviourPun
{
    #region rocket 변수
    float rocketSpeed;
    float rocketRadius = 3;

    Rigidbody rb;
    public GameObject rocketExploImpactFactory;

    public PhotonView tankPv;
    #endregion

    void Start()
    {
        //대충 20m정도 날아감
        //rocketSpeed = Random.Range(10, 20);
        rocketSpeed = Random.Range(10, 15);

        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * rocketSpeed, ForceMode.Impulse);
    }

    void Update()
    {
        //??? 로켓헤드 방향
        transform.forward = rb.velocity.normalized;
    }

    //terrain과 부딪히는 것은 trigger임
    private void OnTriggerEnter(Collider other)
    {
        print("rocket이 Trigger로 부딪힌 것은" + other);

        //PhotonView 붙이기 전 instantiate
        //GameObject rocketExplo = Instantiate(rocketExploImpactFactory);
        //rocketExplo.transform.position = transform.position;
        //rocketExplo.transform.localScale = Vector3.one * 0.5f;

        if (other.gameObject.tag == "Floor")
        {
            if (/*PhotonNetwork.IsMasterClient*/true)
            {
                StartCoroutine(rocketExplodebyTrigger(/*other.transform.up*/));
            }

            Destroy(gameObject);
            EJBossSFX.instance.PlaybombExploSFX();

            //rocket떨어진 위치와 몸체의 위치            
            //GameObject boss = gameObject.GetComponent<BossFSM>().gameObject;
            //print("bomb가 떨어진 지점은 "+ (other.transform.position -boss.transform.position));
        }

        if (other.gameObject.tag == "Player")
        {
            PlayerDamage();
        }
    }

    #region rocketFX

    //bomb의 잔상 켜졌다 꺼지는 함수
    IEnumerator rocketExplodebyTrigger(/*Vector3 normal*/)
    {
        tankPv.RPC("ShowRocketExploImpact", RpcTarget.All, transform.position, 2);

        yield return new WaitForSeconds(3f);
    }
    #endregion

    void PlayerDamage()
    {
        //bomb반경 안의 player damage 
        RaycastHit[] bombHits = Physics.SphereCastAll(transform.position, rocketRadius, Vector3.up, 0f, LayerMask.GetMask("Player"));

        print("bomb에 맞은 것은 " + bombHits[0].transform.gameObject.name);

        foreach (RaycastHit hitObj in bombHits)
        {

            hitObj.transform.GetComponent<PhotonView>().RPC("damaged", RpcTarget.All, hitObj.point, 3);
        }

    }

}
