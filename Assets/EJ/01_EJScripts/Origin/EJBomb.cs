using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class EJBomb : MonoBehaviourPun
{
    #region bomb����
    //bomb ����
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

        //bomb �����Ǹ� AddForce
        bombPower = Random.Range(14f, 20f);
        //rb.velocity = Vector3.zero;
        rb.AddForce(transform.forward * bombPower, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        //bomb head����
        transform.forward = rb.velocity.normalized;
    }



    #region 01. collision �Լ� (�Ⱦ�)

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            if(PhotonNetwork.IsMasterClient)
            {
                //04.coroutine���� ���� �� ������Ʈ�� ���� GameObject�� ���� �Ŀ��� �۵��ϵ��� �Ѵ�.
                StartCoroutine(bombExplode(collision));
            }

            //���� �ε����� bomb ���ֱ�
            EJObjectPoolMgr.instance.ReturnbombQueue(transform.gameObject);
            EJBossSFX.instance.PlaybombExploSFX();
        }       
    }

    #endregion

    #region 01-1. collision�� �� bombExplo (�Ⱦ�) 
    //bomb �ܻ��� ������ ������ �Լ�
    IEnumerator bombExplode (Collision collision)
    {
        tankPv.RPC("ShowBombExploImpact", RpcTarget.All, transform.position, collision.GetContact(0).normal, bombDestroyTime);

        yield return new WaitForSeconds(bombDestroyTime);

        //bomb�ݰ� ���� player damage 
        RaycastHit[] bombHits = Physics.SphereCastAll(transform.position, bombRadius, Vector3.up, 0f, LayerMask.GetMask("Player"));

        foreach (RaycastHit hitObj in bombHits)
        {
            hitObj.transform.GetComponent<PhotonView>().RPC("damaged",RpcTarget.All,hitObj.point, 3);
        }

        yield return null;
    }

    #endregion


    #region 02. Trigger �Լ� 

    private void OnTriggerEnter(Collider other)
    {
        print("bomb���� ���� �ȿ� ���� ����" + other.gameObject);

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

    #region 02-1. Trigger�� �� bombExplo
    //bomb �ܻ��� ������ ������ �Լ�
    IEnumerator bombExplodebyTrigger(Vector3 normal)
    {
        tankPv.RPC("ShowBombExploImpact", RpcTarget.All, transform.position, normal, bombDestroyTime);

        yield return new WaitForSeconds(bombDestroyTime);
    }
    #endregion

    #region 03. bombRadius�� ���� PlayerDamage�Լ�
    void PlayerDamage()
    {
        //bomb�ݰ� ���� player damage 
        RaycastHit[] bombHits = Physics.SphereCastAll(transform.position, bombRadius, Vector3.up, 0f, LayerMask.GetMask("Player"));

        print("bomb�� ���� ���� "+ bombHits[0].transform.gameObject.name);

        foreach (RaycastHit hitObj in bombHits)
        {

            hitObj.transform.GetComponent<PhotonView>().RPC("damaged", RpcTarget.All, hitObj.point, 3);
        }

    }
    #endregion

}

