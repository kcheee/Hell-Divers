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

    #region 01. Triggerȿ��
    //terrain�� �ε����� ���� trigger��
    private void OnTriggerEnter(Collider other)
    {
        print("rocket�� Trigger�� �ε��� ����" + other);

        if (other.gameObject.tag == "Floor")
        {
            if (/*PhotonNetwork.IsMasterClient*/true)
            {
                //StartCoroutine(rocketExplodebyTrigger(/*other.transform.up*/));
                ShowRocketExploImpact(transform.position, 3);

                //����ȭ �ȵǸ� �̰�
                photonView.RPC(nameof(ShowRocketExploImpact), RpcTarget.All, transform.position, 3);

            }

            Destroy(gameObject);
            EJBossSFX.instance.PlaybombExploSFX();
        }

        if (other.gameObject.tag == "Player")
        {
            PlayerDamage();
        }
    }

    #endregion

    #region 02. ShowRocketExploImpact

    [PunRPC]
    public void ShowRocketExploImpact(Vector3 pos, /*Vector3 normal,*/ float waitTime)
    {
        print("������ �ٴ��浹 ȿ���� �߻��߽��ϴ�");

        GameObject rocketExploImpact = Instantiate(rocketExploImpactFactory);
        //GameObject rocketExploImpact = PhotonNetwork.Instantiate("bomb+DAX_Frost_Nova_t0_0 1", pos,Quaternion.identity);

        rocketExploImpact.transform.position = pos;

        StartCoroutine(wait(rocketExploImpact, waitTime));

    }

    IEnumerator wait(GameObject rocket, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }

    #endregion

    #region PlayerDamage�Լ�

    void PlayerDamage()
    {
        //bomb�ݰ� ���� player damage 
        RaycastHit[] bombHits = Physics.SphereCastAll(transform.position, rocketRadius, Vector3.up, 0f, LayerMask.GetMask("Player"));

        print("Rocket�� ���� ���� " + bombHits[0].transform.gameObject.name);

        foreach (RaycastHit hitObj in bombHits)
        {
            hitObj.transform.GetComponent<PhotonView>().RPC("damaged", RpcTarget.All, hitObj.point, 3);
        }

    }
    #endregion

}
