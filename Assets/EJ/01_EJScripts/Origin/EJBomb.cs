using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class EJBomb : MonoBehaviourPun
{
    #region bomb변수
    //bomb 변수
    Rigidbody rb;
    float bombSpeed;
    float bombPower;    
    float bombDestroyTime = 0.2f;
    float bombRadius = 2;

    //photonView
    public PhotonView tankPv;

    //playerDamage
    GameObject player;
    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //bomb 생성되면 AddForce
        bombPower = Random.Range(14f, 20f);
        //rb.velocity = Vector3.zero;
        rb.AddForce(transform.forward * bombPower, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        //bomb head방향
        transform.forward = rb.velocity.normalized;
    }



    #region 01. collision 함수 (안씀)

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            if(PhotonNetwork.IsMasterClient)
            {
                //04.coroutine만을 위한 빈 오브젝트를 만들어서 GameObject가 꺼진 후에도 작동하도록 한다.
                StartCoroutine(bombExplode(collision));
            }

            //땅과 부딪히면 bomb 없애기
            EJObjectPoolMgr.instance.ReturnbombQueue(transform.gameObject);
            EJBossSFX.instance.PlaybombExploSFX();
        }       
    }

    #endregion

    #region 01-1. collision일 때 bombExplo (안씀) 
    //bomb 잔상이 켜졌다 꺼지는 함수
    IEnumerator bombExplode (Collision collision)
    {
        tankPv.RPC("ShowBombExploImpact", RpcTarget.All, transform.position, collision.GetContact(0).normal, bombDestroyTime);

        yield return new WaitForSeconds(bombDestroyTime);

        //bomb반경 안의 player damage 
        RaycastHit[] bombHits = Physics.SphereCastAll(transform.position, bombRadius, Vector3.up, 0f, LayerMask.GetMask("Player"));

        foreach (RaycastHit hitObj in bombHits)
        {
            hitObj.transform.GetComponent<PhotonView>().RPC("damaged",RpcTarget.All,hitObj.point, 3);
        }

        yield return null;
    }

    #endregion


    #region 02. Trigger 함수 

    private void OnTriggerEnter(Collider other)
    {
        print("bomb터짐 범위 안에 들어온 것은" + other.gameObject);

        if (other.gameObject.tag == "Floor")
        {
            if (PhotonNetwork.IsMasterClient)
            {
                StartCoroutine(bombExplodebyTrigger(other.transform.up));
            }

            EJObjectPoolMgr.instance.ReturnbombQueue(transform.gameObject);
            EJBossSFX.instance.PlaybombExploSFX();
        }

        if (other.gameObject.tag == "Player")
        {
            PlayerDamage();
        }
    }
    #endregion

    #region 02-1. Trigger일 때 bombExplo
    //bomb 잔상이 켜졌다 꺼지는 함수
    IEnumerator bombExplodebyTrigger(Vector3 normal)
    {
        tankPv.RPC("ShowBombExploImpact", RpcTarget.All, transform.position, normal, bombDestroyTime);

        yield return new WaitForSeconds(bombDestroyTime);
    }
    #endregion

    #region 03. bombRadius에 들어온 PlayerDamage함수
    void PlayerDamage()
    {
        //bomb반경 안의 player damage 
        RaycastHit[] bombHits = Physics.SphereCastAll(transform.position, bombRadius, Vector3.up, 0f, LayerMask.GetMask("Player"));

        print("bomb에 맞은 것은 "+ bombHits[0].transform.gameObject.name);

        foreach (RaycastHit hitObj in bombHits)
        {

            hitObj.transform.GetComponent<PhotonView>().RPC("damaged", RpcTarget.All, hitObj.point, 3);
        }

    }
    #endregion

}

