using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EJBoss2ndPatternRocket : MonoBehaviourPun
{
    #region rocket ����
    float rocketSpeed;
    float rocketRadius = 3;

    Rigidbody rb;
    public GameObject rocketExploImpactFactory;

    public PhotonView tankPv;
    #endregion

    void Start()
    {
        //���� 20m���� ���ư�
        //rocketSpeed = Random.Range(10, 20);
        rocketSpeed = Random.Range(10, 15);

        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * rocketSpeed, ForceMode.Impulse);
    }

    void Update()
    {
        //??? ������� ����
        transform.forward = rb.velocity.normalized;
    }

    //terrain�� �ε����� ���� trigger��
    private void OnTriggerEnter(Collider other)
    {
        print("rocket�� Trigger�� �ε��� ����" + other);

        //PhotonView ���̱� �� instantiate
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

            //rocket������ ��ġ�� ��ü�� ��ġ            
            //GameObject boss = gameObject.GetComponent<BossFSM>().gameObject;
            //print("bomb�� ������ ������ "+ (other.transform.position -boss.transform.position));
        }

        if (other.gameObject.tag == "Player")
        {
            PlayerDamage();
        }
    }

    #region rocketFX

    //bomb�� �ܻ� ������ ������ �Լ�
    IEnumerator rocketExplodebyTrigger(/*Vector3 normal*/)
    {
        tankPv.RPC("ShowRocketExploImpact", RpcTarget.All, transform.position, 2);

        yield return new WaitForSeconds(3f);
    }
    #endregion

    void PlayerDamage()
    {
        //bomb�ݰ� ���� player damage 
        RaycastHit[] bombHits = Physics.SphereCastAll(transform.position, rocketRadius, Vector3.up, 0f, LayerMask.GetMask("Player"));

        print("bomb�� ���� ���� " + bombHits[0].transform.gameObject.name);

        foreach (RaycastHit hitObj in bombHits)
        {

            hitObj.transform.GetComponent<PhotonView>().RPC("damaged", RpcTarget.All, hitObj.point, 3);
        }

    }

}
